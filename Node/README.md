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

  /info

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

  /blocks

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

  /blocks/:index

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
  Returns json data with requested address balance with requested confirmations accuracy. (in development)
```[GET] /balance/:address/confirmations/:confirmCount``` 


Receive new transaction
----
  Receive new transaction.
```[POST] /transactions/new```


Get transaction info
----
  Returns json data with transaction info. (in development)
```[GET] /transactions/:tranHash/info```


Get pending transactions
----
  Returns json data with node's information.
```[GET] /transactions/pending```

Notify blocks
----
  Returns json data with node's information.
```[POST] /blocks/notify```

Get peers
----
  Returns json data with node's information.
```[GET] /peers```

Add new peer
----
  Receive new peer data.
```[POST] /peers```

Get mining job
----
  Returns json object with new block for mining.
```[GET] /mining/get-block/:address```

Receive Proof-of-Work
----
  Receive Proof-of-Work data.
```[POST] /mining/get-block/:address```
