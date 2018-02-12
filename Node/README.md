# SoftUniChain

SoftUni Blockchain - Node

Starting node:
Step 1: restore required libraries

```npm install```

Step 2: Run node server

```npm start```

Node endpoints are explained below.

Info
----
  Returns json data with node's information.

* **URL**

 `/info`

* **Method:**

  `GET`
  
* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Example Content:**
    
      ```javascript
      {
          "about": "SoftUniChain/0.9-csharp",
          "nodeName": "Sofia-01",
          "peers": [],
          "difficulty": 5,
          "blocks": 43,
          "cummulativeDifficulty": 127,
          "confirmedTransactions": 208,
          "pendingTransactions": 2
      }
      ```
 
* **Error Response:**

  * **Code:** 404 <br />
    **Content:** 
    ```javascript
    { 
      "error" : "Node is offline or something other is wrong!"
    }
    ```


Blocks
----
  Returns json data with node's information.

* **URL**

  `/blocks`

* **Method:**

  `GET`
  
* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Example Content:**
    
      ```javascript
      {
          {
              "index": 0,
              "transactions": [],
              "prevBlockHash": "d279fa6a31ae4fb07cfd9cf7f35cc013cf20a",
              "blockDataHash": "5d845cddcd4404ecfd5476fd6b1cf0ea80cd3",
              "minedBy": "f582d57711a618e69d588ce93895d749858fa95b",
              "nonce": 2455432,
              "difficulty": 5,
              "dateCreated": "2018-02-01T23:23:56.337Z",
              "blockHash": "816534932c2b7154836da6afc367695e6337db8a921823784c14378abed4f7d7"
          },
          {
              "index": 1,
              "transactions": [
                  {
                      "fromAddress": "0x0",
                      "toAddress": "f582d57711a618e69d588ce93895d749858fa95b",
                      "value": 25,
                      "senderPubKey": "",
                      "senderSignature": "",
                      "transactionHash": "",
                      "dateReceived": 1518433285697,
                      "minedInBlockIndex": 1,
                      "paid": false
                  },
                  {
                      "fromAddress": "0x0",
                      "toAddress": "f582d57711a618e69d588ce93895d749858fa95b",
                      "value": 25,
                      "senderPubKey": "",
                      "senderSignature": "",
                      "transactionHash": "",
                      "dateReceived": 1518433290383,
                      "minedInBlockIndex": 1,
                      "paid": false
                  }
              ],
              "prevBlockHash": "1e4024840e9ad3d8be334d8b3cc51ffbfb6beac8d9bff4576dbc335761501bfb",
              "blockDataHash": "4ea5c508a6566e76240543f8feb06fd457777be39549c4016436afda65d2330e",
              "minedBy": "f582d57711a618e69d588ce93895d749858fa95b",
              "nonce": "510982",
              "difficulty": 5,
              "dateCreated": "2018-02-12T11:01:32.2758960Z",
              "blockHash": "0000083FE91D0987AABBBF8A9FC64DF0BC6ABEACC1322EF08B1B849B7355105C"
          },
          {...}, {...}, {...}
      }
      ```
 
* **Error Response:**

  * **Code:** 404 <br />
    **Content:** 
    ```javascript
    { 
      "error" : "Node is offline or something other is wrong!"
    }
    ```

  
Block info by index
----
  Returns json data with block N information.

* **URL**

  `/blocks/:index`

* **Method:**

  `GET`
  
* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Example Content:**
    
      ```javascript
      {
        "index": 1,
        "transactions": [
            {
                "fromAddress": "0x0",
                "toAddress": "f582d57711a618e69d588ce93895d749858fa95b",
                "value": 25,
                "senderPubKey": "",
                "senderSignature": "",
                "transactionHash": "",
                "dateReceived": 1518433285697,
                "minedInBlockIndex": 1,
                "paid": false
            },
            {
                "fromAddress": "0x0",
                "toAddress": "f582d57711a618e69d588ce93895d749858fa95b",
                "value": 25,
                "senderPubKey": "",
                "senderSignature": "",
                "transactionHash": "",
                "dateReceived": 1518433290383,
                "minedInBlockIndex": 1,
                "paid": false
            }
        ],
        "prevBlockHash": "1e4024840e9ad3d8be334d8b3cc51ffbfb6beac8d9bff4576dbc335761501bfb",
        "blockDataHash": "4ea5c508a6566e76240543f8feb06fd457777be39549c4016436afda65d2330e",
        "minedBy": "f582d57711a618e69d588ce93895d749858fa95b",
        "nonce": "510982",
        "difficulty": 5,
        "dateCreated": "2018-02-12T11:01:32.2758960Z",
        "blockHash": "0000083FE91D0987AABBBF8A9FC64DF0BC6ABEACC1322EF08B1B849B7355105C"
      }
      ```
 
* **Error Response:**

  * **Code:** 404 <br />
    **Content:** 
    ```javascript
    { 
      "error" : "Node is offline or something other is wrong!"
    }
    ```

  
Address balance
----
  Returns json data with requested address balance with requested confirmations accuracy.  (in development)
* **URL**

  `/balance/:address/confirmations/:confirmCount`

* **Method:**

  `GET`

* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Content:** 
    ```javascript
    {
      "address": "f582d57711a618e69d588ce93895d749858fa95b",
      "balance": 1239,
      "confirmations": 6
    }
    ```
 
* **Error Response:**

  * **Code:** 404 NOT FOUND <br />
    **Content:** 
    ```javascript
    {
      "error": "Something went wrong!"
    }
    ```


Receive new transaction
----
  Receive new transaction.

* **URL**

  `/transactions/new`

* **Method:**

  `POST`

* **Data Params**

  `from=[string]`
  
  `to=[string]`
  
  `value=[integer]`
  
  `senderPubKey=[string]`
  
  `senderSignature= array<string>`
  
  `dateCreated=[string]`
  
  
  ** Example **
  ```javascript
  {
    "from": "434fe012382830696beb632e24541c3c201e8728276c9ec3fffaf2675",
    "to": "9a9f082f37270ff54c12125ca4204a0e34da695cacc1fe9127",
    "value": 22333,
    "senderPubKey": "2a1d79fb8743d30a4a8501e0028079bcf82a4ddeae1",
    "senderSignature": ["e20a3ec29d3370f79f", "cf90acd0c132ffe56"],
    "dateCreated": "2018-02-01T23:23:56.337Z"
  }
  ```

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```javascript
    {
      "dateReceived": "Mon Feb 12 2018 13:02:57 GMT+0200 (EET)",
      "transactionHash": "fdce3256c70f2329bdcd2a58f8d6a38236207235b15616e2607c2d723c005235"
    }
    ```
 
* **Error Response:**

  * **Code:** 404 <br />
    **Content:** 
    ```javascript
    { 
      "error" : "Node is offline or something other is wrong!"
    }
    ```


Get transaction info
----
  Returns json data with transaction info. (in development)
  
* **URL**

  `/transactions/:transactionsHash/info`

* **Method:**

  `GET`

* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```javascript
    {
        "fromAddress": "0x0",
        "toAddress": "f582d57711a618e69d588ce93895d749858fa95b",
        "value": 25,
        "senderPubKey": "",
        "senderSignature": "",
        "transactionHash": "",
        "dateReceived": 1518433635353,
        "minedInBlockIndex": 42,
        "paid": false
    }
    ```
 
* **Error Response:**

  * **Code:** 404 NOT FOUND <br />
    **Content:**
    ```javascript
    {
      "error" : "Transaction does not exist!"
    }
    ```


Get pending transactions
----
  Returns json data with pending transactions.
  
* **URL**

  `/transactions/pending`

* **Method:**

  `GET`

* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Content:** 
    ```javascript
    {
      {
          "fromAddress": "0x0",
          "toAddress": "f582d57711a618e69d588ce93895d749858fa95b",
          "value": 25,
          "senderPubKey": "",
          "senderSignature": "",
          "transactionHash": "",
          "dateReceived": 1518432018293,
          "minedInBlockIndex": 1,
          "paid": false
      },
      {
          "fromAddress": "0x0",
          "toAddress": "f582d57711a618e69d588ce93895d749858fa95b",
          "value": 25,
          "senderPubKey": "",
          "senderSignature": "",
          "transactionHash": "",
          "dateReceived": 1518432022968,
          "minedInBlockIndex": 1,
          "paid": false
      },
      {...}, {...}, {...}
    }
    ```
 
* **Error Response:**

  * **Code:** 404 NOT FOUND <br />
    **Content:** 
    ```javascript
    {
      "error": "Something went wrong!"
    }
    ```
  

Notify blocks
----
  Returns json data with node's information.
  
```[POST] /blocks/notify```


Get peers
----
  Returns json data with peers information.

* **URL**

  `/peers`

* **Method:**

  `GET`
  
* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```javascript
    {
      "http://192.168.0.1",
      "http://192.168.0.2",
      "http://192.168.0.3"
    }
    ```
 
* **Error Response:**

  * **Code:** 404 NOT FOUND <br />
    **Content:** 
    ```javascript
    {
      "error": "Something went wrong!"
    }
    ```


Add new peer
----
  Receive json data with new peer info.

* **URL**

  `/peers`

* **Method:**

  `POST`
  
* **Data Params**

  `url=[string]`

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```javascript
    {
      "message": "Added peer: http://192.168.112.123"
    }
    ```
 
* **Error Response:**

  * **Code:** 404 NOT FOUND <br />
    **Content:** 
    ```javascript
    {
      "error": "Something went wrong!"
    }
    ```

Get mining job
----
  Returns json object with new block for mining.

* **URL**

  `/mining/get-block/:address`

* **Method:**

  `GET`
  
* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```javascript
    {
      "index": 1,
      "transactionsHash": "4ea5c508a6566e76240543f8feb06fd457777be39549c4016436afda65d2330e",
      "prevBlockHash": "1e4024840e9ad3d8be334d8b3cc51ffbfb6beac8d9bff4576dbc335761501bfb",
      "difficulty": 5
    }
    ```
 
* **Error Response:**

  * **Code:** 404 NOT FOUND <br />
    **Content:**
    ```javascript
    {
      "error": "Error!"
    }
    ```


Receive Proof-of-Work
----
  Receive Proof-of-Work data.

* **URL**

  `/mining/get-block/:address`

* **Method:**

  `POST`
  
* **Data Params**

  `nonce=[integer]`
  
  `blockHash=[string]`
  
  `dateCreated=[timestamp]`

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```javascript
    {
      "nonce": 246330,
      "blockHash": "0000045ED509048696CDFD9BBFDEDEC393E9EA144FE74BA7A7A4EDA3245FBC3B",
      "dateCreated": "2018-02-12T11:07:16.2021920Z"
    }
    ```
 
* **Error Response:**

  * **Code:** 404 NOT FOUND <br />
    **Content:**
    ```javascript
    {
      "error": "Something went wrong!"
    }
    ```
