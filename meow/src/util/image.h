/* - image.h ---------------------------------------------------------------------------------------
 * Low-level API to work with images
 *
 * This file is a part of MEOW project.
 *
 * This software is in public domain, distributed on "AS IS" basis, without technical support,
 * and with no warranty, epress or implied, as to its usefulness for any purpose.
 *
 * Copyright (c) Grim Maple @ 2017
 * -------------------------------------------------------------------------------------------------
 */

#ifndef _IMAGE_H_
#define _IMAGE_H_

#include "../api.h"

#define IMAGE_RGBA 4
#define IMAGE_RGB  3
#define IMAGE_R    1

typedef struct img 
{
	int32  type;
	word   width;
	word   height;
	void  *data;
} Image;

typedef Image* image_ptr;


image_ptr MEOW_API image_create( dword, dword );
image_ptr MEOW_API image_create_type( dword, dword, dword );
image_ptr MEOW_API image_create_from_data( byte*, int32, int32, int32);
void      MEOW_API image_free( Image* );
dword     MEOW_API image_get_type( Image* );
int32     MEOW_API image_get_pixel( Image*, int32, int32 );
void      MEOW_API image_set_pixel( Image*, int32, int32, int32 );
void      MEOW_API image_set_subimage( Image*, Image*, int32, int32 );
image_ptr MEOW_API image_subimage( Image*, int32, int32, int32, int32 );
mem_ptr   MEOW_API image_data( Image*, int32* );
dword     MEOW_API image_data_set( Image*, byte*, int32 );

dword	  MEOW_API image_get_width( Image* src );
dword	  MEOW_API image_get_height( Image *src );

#endif