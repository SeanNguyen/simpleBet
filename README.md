# SimpleBet
![alt text](http://arcadier.com/wp-content/themes/Arcadier/images/logo.png "Arcadier")

### Overview:
This project apply two core technologies which is ASP.NET and AngularJS. As a result the implementation is very different from the traditional ASP.NET project. We know, this sound like a formula of disaster when trying to mix 2 different framework together but continue to read on.

About AngularJS: This is a very powerful front-end framework from Google. Fully support 2-ways data binding and is a perfect candidate for making a MVVM modern web application. In another way, Angular + HTML + ASP.NET are similar to WPF + XAML + .NET.

![](https://cloud.githubusercontent.com/assets/5309295/7768666/15d696fe-00b0-11e5-9f9d-336c85f4d2c0.png)

The reason we want to apply this because we want to make an Single Page Application (SPA) which AngularJs was made for. Beside that we also want to separate the concerns of the server and the front-end. Let the server do the server work and serve as a Web API service instead of worries about rendering pages. And let client do front-end work like manage html, DOM, animation, display infomation.

Actually the result turn out quite good when we realise that Microsoft officially natively support Angular JS and SPA in its latest ASP.NET version - ASP.NET 5 (ASP.NET vNext). Together with it, ASP.NEt 5 also support all tool that come a long with AngularJS and NodeJS eco-system like Grunt, Glup, npm, bower.

More a more inspiring demo from Microsoft themselve, watch this:
[https://www.youtube.com/watch?v=f67PFtrldGQ](https://www.youtube.com/watch?v=f67PFtrldGQ)


####For set up guide, please refer to this wiki page:
#####[GETTING STARTED](https://github.com/SeanNguyen/gambling/wiki/Getting-Started)
