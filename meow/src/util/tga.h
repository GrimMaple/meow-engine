/* - tga.h ----------------------------------------------------------------------------------------
* Low-level TAGRA image processing
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2018
* -------------------------------------------------------------------------------------------------
*/

#ifndef _TGA_H_
#define _TGA_H_

#include "image.h"

image_ptr MEOW_API tga_load( string path );

#endif