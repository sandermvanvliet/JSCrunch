# JSCrunch

[![Build status](https://ci.appveyor.com/api/projects/status/h3hvd85d47e8rtqx?svg=true)](https://ci.appveyor.com/project/sandermvanvliet/jscrunch)

A continuous test runner for Javascript/Typescript using Chutzpah and Jasmine inspired by the wonderful [NCrunch](http://www.ncrunch.net/) by Remco Mulder.

### Usage
* Clone sources
* Update `app.config` in the JSCrunch project and set the following values:
    * `JSCrunch.PathToWatch`: Where to look for changes
    * `JSCrunch.TestPattern`: The file pattern to look for
    * `JSCrunch.TestRunnerExecutable`: The path to Chutzpah
    * `JSCrunch.TestRunnerParameters`: Don't change this unless you want to run JavaScript instead of TypeScript
* Start the JSCrunch project

### Some warnings
Currently this is not even alpha, it's in continous hacking state so YMMV.