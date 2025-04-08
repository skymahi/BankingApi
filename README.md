Points to note and run

1. Required to change the database connection as per the running system sql instance
2. Default database values will be seeded after updating the database from the visual studio
3. Run the solution from the visual studio code
4. We can test the API Urls individually by using postman
5. Change the port number if required
6. Set the bearer token for authentication to each request except the login api

a.) Authentication API which returns token

URL : https://localhost:7016/api/auth/login

b.) Get Balance which returns the Balance amount

URL : https://localhost:7016/api/banking/balance

c.) Deposit Amount which adds the amount and returns the Balance amount

URL : https://localhost:7016/api/banking/deposit

Payload

{
    "Amount": 500.00
}

d.) Withdraw Amount which adds the amount and returns the Balance amount

URL : https://localhost:7016/api/banking/withdraw

Payload

{
    "Amount": 100.00
}

All the above api will get the response except the login api as like below

{
    "balance": 500.00
}
