## Git platforms issues manager
Primitive library and REST API client to manage issues on Git platforms (supported so far: GitHub and GitLab)

###  How to run
* Clone repository
* In _Client_ project, for each platform, find an array of type _RequestHeaders_ and enter your Authorization key in file _platforms.json_, exactly here:
  ```json
  {
        "Name": "Authorization",
        "Value": "Bearer <<ENTER_YOUR_API_KEY_HERE>>"
  }
  ```
* run _Client_ project using command:
  ```dotnet run```
  or alternatively:
  ```dotnet watch run```
* navigate in your internet browser to the address:
  ```http://localhost:2137/swagger```

