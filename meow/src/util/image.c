/* - image.c ---------------------------------------------------------------------------------------
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

#include <stdlib.h>
#include <string.h>

#include "image.h"

dword ColorSize( dword type)
{
	switch( type )
	{
	case IMAGE_R:
		return 1;
	case IMAGE_RGB:
		return 3;
	case IMAGE_RGBA:
		return 4;
	default:
		return -1;
	}
}

image_ptr MEOW_API image_create( dword width, dword height )
{
	return image_create_type( width, height, IMAGE_RGBA );
}

image_ptr MEOW_API image_create_type( dword width, dword height, dword type )
{
	if( type > IMAGE_R && type < IMAGE_RGBA)
		return NULLPTR;

	Image *ret = malloc( sizeof( Image ) );
	if ( !ret )
		return NULLPTR;
	dword size = ColorSize( type );
	ret->data = malloc( size * width * height );
	ret->width = width;
	ret->height = height;
	ret->type = type;

	return ret;
}

image_ptr MEOW_API image_create_from_data( byte* data, int32 width, int32 height, int32 type)
{
	Image* image = malloc( sizeof(Image) );
	if ( !image )
		return NULLPTR;
	image->data = malloc( width*height*type );
	if ( !image->data )
	{
		image_free( image );
		return NULLPTR;
	}
	memcpy( image->data, data, width*height*type );
	image->width = width;
	image->height = height;
	image->type = type;
	return image;
}

void MEOW_API image_free( Image* image )
{
	if( image->data )
		free( image->data );
	free( image );
}

dword MEOW_API image_get_type( Image* image )
{
	return image->type;
}

int32 MEOW_API image_get_pixel( Image* image, int32 x, int32 y )
{
	switch ( image->type )
	{
	case IMAGE_RGBA:
		return ((int32*)image->data)[ y*image->width + x ];
	case IMAGE_RGB:
		return (int32)( (byte*)image->data )[ y*( image->width ) * 3 + x * 3 ] | 0x000000FF;
	case IMAGE_R:
		return (int32)( (byte*)image->data )[ y*image->width + x ];
	default:
		return 0;
	}
}

void MEOW_API image_set_pixel( Image* image, int32 x, int32 y, int32 color )
{
	int32 *t;
	switch( image->type )
	{
	case IMAGE_RGBA:
		((int32*)image->data)[y*image->width + x] = color;
		break;
	case IMAGE_RGB:
		t = (int32*)(((byte*)image->data) + (y*image->width*3 + x*3));
		*t |= color & 0xFFFFFF00;
		break;
	case IMAGE_R:
		((byte*)image->data)[y*image->width + x] = (color & 0x000000FF);
		break;
	}
}

void MEOW_API image_set_subimage( Image* dest, Image* source, int32 x, int32 y )
{
	for(int i=0; i<source->height; i++)
	{
		int32 ty = y+i;
		if(ty >= dest->height)
			break;
		for(int j=0; j<source->width; j++)
		{
			int32 tx = x+j;
			if(tx>=dest->width)
				break;
			int32 color = image_get_pixel( source, j, i );
			image_set_pixel( dest, tx, ty, color);
		}
	}
}

image_ptr MEOW_API image_subimage( Image* source, int32 x, int32 y, int32 w, int32 h)
{
	Image* result = image_create_type(w, h, source->type);
	if ( !result )
		return NULLPTR;
	result->width = w;
	result->height = h;
	result->data = malloc( w*h*source->type );
	for(int32 i=0; i<h; i++)
	{
		for(int32 j = 0; j<w; j++)
		{
			image_set_pixel( result, j, i, image_get_pixel( source, x + j, y + i ) );
		}
	}
	return result;
}

mem_ptr MEOW_API image_data( Image* source, int32* refLen )
{
	*refLen = source->height * source->width * source->type;
	return source->data;
}

dword MEOW_API image_data_set( Image* source, byte* data, int32 length )
{
	int32 refLenfth = source->width * source->height * source->type;
	if ( refLenfth != length )
		return 0;
	free( source->data );
	source->data = data;
	memcpy( source->data, data, length );
	return 1;
}

dword MEOW_API image_get_width( Image* src )
{
    return src->width;
}

dword MEOW_API image_get_height( Image* src )
{
    return src->height;
}