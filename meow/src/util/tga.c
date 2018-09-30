/* - tga.c ----------------------------------------------------------------------------------------
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

#include <stdio.h>
#include <stdlib.h>

#include "tga.h"

typedef enum TGA_ORIGIN
{
    TOP_LEFT = 32,
    TOP_RIGHT = 48,
    BOTTOM_LEFT = 0,
    BOTTOM_RIGHT = 16
} tga_origin;

#pragma pack(push, 1)
typedef struct TGA_HEADER
{
    byte idlength;
    byte colourmaptype;
    byte datatypecode;
    short colourmaporigin;
    short colourmaplength;
    byte colourmapdepth;
    short x_origin;
    short y_origin;
    short width;
    short height;
    byte bitsperpixel;
    byte imagedescriptor;
} tga_header;
#pragma pack(pop)

inline bool IsValidHeader( tga_header* header )
{
    return ( ( header->idlength &
        header->colourmaptype &
        header->colourmaporigin &
        header->colourmaplength &
        header->x_origin &
        header->y_origin ) == 0  && ( header->datatypecode == 2 ) );
}

image_ptr ReadBGR( FILE* f, tga_header* hdr )
{
    int32 origin = ( hdr->imagedescriptor & 48 );
    int32 bytes = hdr->bitsperpixel / 8;
    byte *data = malloc( hdr->height * hdr->width * bytes );
    fread( data, 1, hdr->width * hdr->height * bytes, f );
    int32 sx = ( origin == TOP_LEFT || origin == BOTTOM_LEFT ) ? 0 : ( hdr->width - 1 ) * bytes;
    int32 sy = ( origin == TOP_LEFT || origin == TOP_RIGHT ) ? 0 : hdr->height - 1;
    int32 incx = ( origin == TOP_LEFT || origin == BOTTOM_LEFT ) ? 1 : -1;
    int32 incy = ( origin == TOP_LEFT || origin == TOP_RIGHT ) ? 1 : -1;
    int32 x = 0;

    image_ptr ret = image_create_type( hdr->width, hdr->height, bytes );

	int32 c = 0;

    do
    {
        x = sx;
        do
        {
            int32 colorCode = (data[ c + 2 ] | ( data[ c + 1 ] << 8 ) | ( data[ c ] << 16 ) | ( 0xFF << 24 ));
			if (bytes == 4)
				colorCode |= (data[c + 3] << 24);
			image_set_pixel(ret, x, sy, colorCode);
            x += incx;
			c += bytes;
        } while ( x >= 0 && x < hdr->width);
        sy += incy;
    } while ( sy >= 0 && sy < hdr->height );

	free(data);

	return ret;
}

image_ptr MEOW_API tga_load( string path )
{
    FILE* file = fopen( path, "rb" );
    if ( file == NULL )
        return NULLPTR;
	image_ptr ret = NULLPTR;
    tga_header hdr;
    int read = fread( &hdr, sizeof( tga_header ), 1, file );
    if ( read < 1 )
        goto failure;
    if ( !IsValidHeader( &hdr ) )
        goto failure;
    ret = ReadBGR( file, &hdr );

failure:
    fclose( file );
    return ret;

}