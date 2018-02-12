using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Miner
{
    class Program
    {
        static void Main(string[] args)
        {
            // Mocked response
            //            String responseFromNode = @"{
            //'index':1,
            //'expectedReward':62.25,
            //'transactionsHash':'9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08',
            //'prevBlockHash':'816534932c2b7154836da6afc367695e6337db8a921823784c14378abed4f7d7',
            //'difficulty':5
            //}";

            //String nodeIpAddress = args[0];
            //String minerAddress = args[1];


            String nodeIpAddress = "http://localhost:5555";
            String minerAddress = "f582d57711a618e69d588ce93895d749858fa95b";


            WebResponse response = null;
            HttpStatusCode statusCode = HttpStatusCode.RequestTimeout;

            Stopwatch sw = new Stopwatch();
            TimeSpan maxTaskLength = new TimeSpan(0, 0, 5); // 5 seconds

            do
            {
                sw.Start();

                do
                {
                    try
                    {
                        statusCode = HttpStatusCode.RequestTimeout;

                        // Create a request to Node   
                        WebRequest request = WebRequest.Create(nodeIpAddress + "/mining/get-block/" + minerAddress);
                        request.Method = "GET";
                        request.Timeout = 3000;
                        request.ContentType = "application/json; charset=utf-8";

                        response = request.GetResponse();
                        statusCode = ((HttpWebResponse)response).StatusCode;
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine("WebException raised!");
                        Console.WriteLine("{0}\n", e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception raised!");
                        Console.WriteLine("Source : {0}", e.Source);
                        Console.WriteLine("Message : {0}\n", e.Message);
                    }
                } while (statusCode != HttpStatusCode.OK);

                // Get the stream containing content returned by the Node.
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.
                string responseFromNode = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                BlockTemplate blockTemplate = JsonConvert.DeserializeObject<BlockTemplate>(responseFromNode);

                Console.WriteLine("\nStart new task:");
                Console.WriteLine("Index of block to mine: {0}", blockTemplate.Index);
                Console.WriteLine("Expected Reward: {0}", blockTemplate.ExpectedReward);
                Console.WriteLine("TransactionsHash: {0}", blockTemplate.TransactionsHash);
                Console.WriteLine("PrevBlockHash: {0}", blockTemplate.PrevBlockHash);
                Console.WriteLine("Difficulty: {0}\n", blockTemplate.Difficulty);

                Boolean blockFound = false;
                UInt64 nonce = 0;
                String timestamp = DateTime.UtcNow.ToString("o");
                String difficulty = new String('0', blockTemplate.Difficulty) +
                    new String('9', 64 - blockTemplate.Difficulty);

                // blockHash = SHA256(Index|TransactionsHash|PrevBlockHash|TimeStamp|Nonce);

                String precomputedData = blockTemplate.Index.ToString()
                        + blockTemplate.TransactionsHash
                        + blockTemplate.PrevBlockHash;
                String data;
                String blockHash;

                while (!blockFound && nonce < UInt32.MaxValue)
                {
                    data = precomputedData + timestamp + nonce.ToString();

                    blockHash = ByteArrayToHexString(Sha256(Encoding.UTF8.GetBytes(data)));


                    if (String.CompareOrdinal(blockHash, difficulty) < 0)
                    {
                        Console.WriteLine("!!! Block found !!!");
                        Console.WriteLine($"Block Hash: {blockHash}\n");

                        JObject obj = JObject.FromObject(new
                        {
                            nonce = nonce.ToString(),
                            dateCreated = timestamp,
                            blockHash = blockHash
                        });

                        Byte[] blockFoundData = Encoding.UTF8.GetBytes(obj.ToString());
                        Int32 retries = 0;

                        do
                        {
                            try
                            {
                                statusCode = HttpStatusCode.RequestTimeout;

                                WebRequest request = WebRequest.Create(nodeIpAddress + "/mining/get-block/" + minerAddress);
                                request.Method = "POST";
                                request.Timeout = 3000;
                                request.ContentType = "application/json; charset=utf-8";

                                Console.WriteLine($"PrecomputedData: {precomputedData}");
                                Console.WriteLine($"Timestamp: {timestamp}");
                                Console.WriteLine($"Nonce: {nonce.ToString()}");

                                dataStream = request.GetRequestStream();
                                dataStream.Write(blockFoundData, 0, blockFoundData.Length);
                                dataStream.Close();

                                response = request.GetResponse();
                                statusCode = ((HttpWebResponse)response).StatusCode;

                                // Display the status.
                                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                                response.Close();
                            }
                            catch (WebException e)
                            {
                                Console.WriteLine("WebException raised!");
                                Console.WriteLine("{0}\n", e.Message);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Exception raised!");
                                Console.WriteLine("Source : {0}", e.Source);
                                Console.WriteLine("Message : {0}\n", e.Message);
                            }

                            System.Threading.Thread.Sleep(1000);
                        } while (statusCode != HttpStatusCode.OK && retries++ < 3);

                        blockFound = true;
                        break;
                    }

                    // print intermediate data

                    if (nonce % 1000000 == 0)
                    {
                        Console.WriteLine(timestamp);
                        Console.WriteLine($"Nonce: {nonce}");
                        Console.WriteLine($"Block Hash: {blockHash}\n");
                    }

                    // get new timestamp on every 100000 iterations

                    if (nonce % 100000 == 0)
                    {
                        timestamp = DateTime.UtcNow.ToString("o");
                    }

                    nonce++;

                    if (maxTaskLength < sw.Elapsed)
                    {
                        sw.Reset();
                        break;
                    }
                }
            } while (true);
        }

        public static byte[] Sha256(byte[] array)
        {
            SHA256Managed hashstring = new SHA256Managed();
            return hashstring.ComputeHash(array);
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            string hexAlphabet = "0123456789ABCDEF";

            foreach (byte b in bytes)
            {
                result.Append(hexAlphabet[(int)(b >> 4)]);
                result.Append(hexAlphabet[(int)(b & 0x0F)]);
            }

            return result.ToString();
        }
    }
}
