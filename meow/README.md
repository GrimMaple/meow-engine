# meow  

Low-level end of the meow-engine. Written in Pure C.  

## Purpose  

- Operate with syscalls and some other low-level calls to not waste time  
- Maintane system's window creation and lifecycle  
- Organize callbacks and raise events  
- Provide cross-platform support (will be implemented in near future)  

## Structure  

### mio  

As in Meow Input/Output  
- Provides input support for XInput or any other kind of input/output  
  
### util  

Provides utility and any helper code  
  
### windows  

Windows-specific implementations  
  
### linux  

Linux-specific implementations