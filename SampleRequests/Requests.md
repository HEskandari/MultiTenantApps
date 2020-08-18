## Starting the service

The service runs on http://localhost:5001. There is a API controller for order creation (`OrderController`).

The sample below uses HTTPie client to send the requests.

### Shoe Store Tenant
The tenant 100 is a shoe store. It has the following products:

| ProductID | Name                    | 
| ----------| ----------------------- |
| 1         | Mens Santa Cruz Loafers | 
| 2         | Road Running Shoes      |

To create a shoe order:

```
http post http://localhost:5001/order/shoestore/create Customer="+610423443125" ProductId=1 Quantity=1
```

### Watch Store Tenant
The tenant 200 is a watch store. It has the following products:

| ProductID | Name                              | 
| ----------| --------------------------------- |
| 1         | G-Shock GA-100CM-4A XL Camouflage | 
| 2         | Diesel Chief Series Analog        |

To create a watch order:

```
http post http://localhost:5001/order/watchstore/create Customer="someone@gmail.com" ProductId=2 Quantity=5
```

