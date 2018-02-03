# UnidaysApp

## Project Structure
There are three projects in the Visual Studio solution. 
### UnidaysApp
This is the Web API. The app is configured to run on http://localhost:57741 by default. 
There is only one method accessible from the Web API which is a POST to http://localhost:57741/api/user 
The request takes a JSON object in the body that defines the user to add to the database, eg.
```
{
	"email":"testuser@gmail.com",
	"password":"testtest"
}
```
The Web API will validate the following 

* The email address is a valid email format
* The password is at least 8 characters long
* The email address does not already exist in the database

If all conditions are passed then the password is encrypted with salted PBKDF2 algorithm. The algorithm is used with SHA256 and 5000 iterations. This should give a good balance between a secure password and a speedy computation should it ever be used for logging users in.

The file Resource.resx contains the path to the SQL server. Ideally this should be moved elsewhere, perhaps into appsettings.json for security.

### UnidaysSQL
This project contains the SQL schema for the database. It should create one table for users with fields for a unique ID, email, encrypted password and salt

### UnitTestProject
Unit tests have been implemented to verify the following

* New user can be added without error
* Input is rejected if the password is too short
* Input is rejected if the email address is not an email
* Input is rejected if the user already exists in the database

## Client
In addition to the Web API in the Visual Studio solution, there is a folder containing the files for the web client. This is a single html file with a sign up form on it. The form uses Bootstrap for templating, jQuery for posting the data to the Web API and hyperform for validation.

The web client will validate as much as possible before sending to the server. This gives a faster response to the user and reduces load on the server. The client will validate 

* The email address is a valid email format
* The password is at least 8 characters long
* The password matches the confirm password

It is assumed that were this to be deployed in a real environment that the application would be served over TLS and that comms to the backend would equally be over TLS should the endpoint be on a separate host.
