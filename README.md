<div align=center>

# [Andy's Bakery](https://andysbakery.azurewebsites.net)

#### Bakery website 09.29.2020

#### By _**Andrew Philpott**_

[Description](#description) | [User Stories](#user-stories) | [Routes](#routes) | [Technologies Used](#technologies-used) | [License](#license)

</div>

## Description

C#/.NET MVC website that allows users to view the different types of flavors and treats offered at Andys. Users have access to CRUD operations for orders and Admin users have the added
functionality of being able to create, edit, and delete flavors and treats.

Ability to use CRUD operations for treat and flavor requires user to login with Email: admin@gmail<span>.com</span> Password: admin

## User Stories

- As an anonymous user I should be able to view a list of all the flavors.
- As an anonymous user I should be able to view a list of all the treats.
- As an anonymous user I should be able to view a list of flavors for a treat.
- As an anonymous user I should be able to view a list of treats for a flavor.
- As an anonymous user I should be able to view a list of all the flavors and treats offered at the bakery on the same page.
- As an anonymous user I should be able to register an account.
- As an anonymous user I should be able to login to my account.
- As an anonymous user I should be able to logout of my account.
- As a logged in user I should be able to view my orders.
- As a logged in user I should be able create, delete, or edit my orders.
- As an admin I should be able to create, delete, or edit a flavor from the list of flavors.
- As an admin I should be able to create, delete, or edit a treat from the list of treats.

## Routes

| Controller | Route Name      | URL Path           | HTTP Method | Purpose                                                                  |
| ---------- | --------------- | ------------------ | ----------- | ------------------------------------------------------------------------ |
| Home       | Index           | /                  | GET         | Homepage displays welcome message & links to the rest of the application |
| Home       | Ourstory        | /ourstory          | GET         | History of the company                                                   |
| Home       | Contactus       | /contactus         | GET         | Offers form to email the company                                         |
| Home       | Menu            | /menu              | GET         | Displays a list of flavors and treats                                    |
| Account    | Register        | /register          | GET         | Offers form to allow a user to register                                  |
| Account    | Register        | /register          | POST        | Create a new user                                                        |
| Account    | Login           | /login             | GET         | Offers form to allow a user to log in                                    |
| Account    | Login           | /login             | POST        | Authenticates the user                                                   |
| Account    | Index           | /account           | GET         | Account page displaying user options                                     |
| Account    | logoff          | /account           | POST        | Logs the user out                                                        |
| Treats     | Index           | /treats            | GET         | Displays list of all treats                                              |
| Treats     | Create          | /treats/create     | GET         | Offers a form to create a treat                                          |
| Treats     | Create          | /treats            | POST        | Create a new treat object                                                |
| Treats     | Details         | /treats/id         | GET         | Displays details of a specific treat                                     |
| Treats     | Delete          | /treats/delete/id  | Get         | Offers a from to delete a treat                                          |
| Treats     | DeleteConfirmed | /treats/id         | POST        | Delete a specific treat                                                  |
| Flavors    | Index           | /flavors           | GET         | Displays list of all flavors                                             |
| Flavors    | Create          | /flavors/create    | GET         | Offers a form to create a flavor                                         |
| Flavors    | Create          | /flavors           | POST        | Create a new flavor object                                               |
| Flavors    | Details         | /flavors/id        | GET         | Displays details of a specific flavor                                    |
| Flavors    | Delete          | /flavors/delete/id | Get         | Offers a from to delete a flavor                                         |
| Flavors    | DeleteConfirmed | /flavors/id        | POST        | Delete a specific flavor                                                 |
| Orders     | Index           | /orders            | GET         | Displays list of all orders                                              |
| Orders     | Create          | /orders/create     | GET         | Offers a form to create an order                                         |
| Orders     | Create          | /orders            | POST        | Create a new order object                                                |
| Orders     | Details         | /orders/id         | GET         | Displays details of a specific order                                     |
| Orders     | Delete          | /orders/id/delete  | GET         | Offers a from to delete an order                                         |
| Orders     | DeleteConfirmed | /orders/id         | POST        | Delete a specific order                                                  |

## Technologies Used

- JavaScript
- C#
- .NET Core
- Azure App Service
- Azure Sql Database
- Azure Sql Server

### License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN\*

Copyright (c) 2020 **Andrew Philott**
