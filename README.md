# meow-engine
This repository contains code of my former and dead engine, MEOW that I previously removed from github.  
  
It seems that I don't really have time to rewrite it, as I planned in near future, so I'm reuploading it in archive purposes.  
  
## Structure  
  
### meow  
  
Low-level C core of the engine  
  
### meow-sharp  
  
C# Framework  
  
## Building  
  
Requries dotnet and basic make tools. Make the C core first. Build samples. Put the resulting libmeow.so within built samples, then use dotnet to run the samples. 