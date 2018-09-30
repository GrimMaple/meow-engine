/* - renderer.c ------------------------------------------------------------------------------------
* OpenGL manipulation implementation
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2016-2017
* --------------------------------------------------------------------------------------------------
*/

#include <stdio.h>
#include <stdlib.h>

#ifdef _WINDOWS
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#endif

#include <GL/gl.h>
#include <GL/glu.h>

#include "renderer.h"
#include "extensions.h"

#define TRACE_ERROR(s) HasError(s)

bool texturesEnabled = FALSE;
bool shouldClear = TRUE;
GLenum error;
float clearColor[ ] = { 0.0, 0.0, 0.0, 1.0 };
dword currentProgram = 0;

enum
{
	METHOD_SQUARES = 4,
	METHOD_TRIANGLES = 3,
	METHOD_LINES = 2,
	METHOD_POINTS = 1
};

void Begin( )
{
	if( shouldClear )
		glClear( GL_COLOR_BUFFER_BIT );

	glLoadIdentity( );
}

void HasError( string t )
{
	static GLenum error = 0;
	error = glGetError();
	if(error)
		printf("Error at %s: %#X\n", t, error);
}

GLenum GetGLMode( dword mode )
{
	GLenum am;
	switch ( mode )
	{
	case METHOD_SQUARES:
		am = GL_QUADS;
		break;
	case METHOD_TRIANGLES:
		am = GL_TRIANGLES;
		break;
	case METHOD_LINES:
		am = GL_LINES;
		break;
	default:
		am = GL_POINTS;
		break;
	}

	return am;
}

void RendererSetup( dword w, dword h )
{
	glViewport( 0, 0, w, h );

	glClearColor( clearColor[ 0 ], clearColor[ 1 ], clearColor[ 2 ], clearColor[ 3 ] );
	glBlendFunc( GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA );
	glEnable( GL_BLEND );
	glEnableClientState( GL_VERTEX_ARRAY );
	glEnableClientState( GL_COLOR_ARRAY );
	glEnableClientState( GL_TEXTURE_COORD_ARRAY );
	glMatrixMode( GL_PROJECTION );
	glLoadIdentity( );
	glOrtho( 0, w, h, 0, 0, 500 );
	glMatrixMode( GL_MODELVIEW );
	glLoadIdentity( );
}

dword MEOW_API renderer_load_texture( Image* image )
{
	GLenum tm = 0;
	switch( image->type )
	{
	default:
	case IMAGE_RGBA:
		tm = GL_RGBA;
		break;
	case IMAGE_RGB:
		tm = GL_RGB;
		break;
	case IMAGE_R:
		tm = GL_RED;
		break;
	}
	dword ret;
	glGenTextures( 1, &ret );
	TRACE_ERROR( "glGenTextures" );
	glBindTexture( GL_TEXTURE_2D, ret );
	TRACE_ERROR( "glBindTexture" );
	glTexImage2D( GL_TEXTURE_2D, 0, 4, image->width, image->height, 0, tm, GL_UNSIGNED_BYTE, image->data );
	TRACE_ERROR( "glTexImage2D" );
	glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST );
	glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST );
	return ret;
}

void MEOW_API renderer_free_texture( dword id )
{
	glDeleteTextures( 1, &id );
}

void MEOW_API renderer_draw_array( float *vertex, float *texture, float *color, dword count, dword mode )
{
	renderer_draw_array_offs( vertex, texture, color, 0, count, mode );
}

void  MEOW_API renderer_draw_array_offs( float *vertex, float *texture, float *color, dword offset, dword count, dword mode)
{
	ResetVBOState( );
	GLenum am = GetGLMode( mode );

	vertex += 3 * offset;
	texture += 2 * offset;
	color += 4 * offset;

	glVertexPointer( 3, GL_FLOAT, 0, vertex );
	if ( color )
		glColorPointer( 4, GL_FLOAT, 0, color );
	if ( texture )
		glTexCoordPointer( 2, GL_FLOAT, 0, texture );

	glDrawArrays( am, 0, count );
}

void MEOW_API renderer_set_texture( unsigned int texid )
{
	if ( currentProgram )
	{
		ActiveTexture( 0 );
	}
	if ( texid && !texturesEnabled )
	{
		glEnable( GL_TEXTURE_2D );
		texturesEnabled = TRUE;
		//glEnableClientState( GL_TEXTURE_COORD_ARRAY );
	}
	if ( !texid && texturesEnabled )
	{
		glDisable( GL_TEXTURE_2D );
		texturesEnabled = FALSE;
		//glDisableClientState( GL_TEXTURE_COORD_ARRAY );
	}
	//if ( texid )
	//TRACE_ERROR( "glDisable" );
		glBindTexture( GL_TEXTURE_2D, texid );
		TRACE_ERROR( "glBindTexture" );
}

void MEOW_API renderer_set_clear_color( float r, float g, float b, float a )
{
	clearColor[ 0 ] = r;
	clearColor[ 1 ] = g;
	clearColor[ 2 ] = b;
	clearColor[ 3 ] = a;
	glClearColor( r, g, b, a );
}

dword MEOW_API renderer_get_max_texture_size( void )
{
	dword sz;
	glGetIntegerv( GL_MAX_TEXTURE_SIZE, &sz );
	return sz;
}

void MEOW_API renderer_set_should_clear( bool sc )
{
	shouldClear = sc;
}

bool MEOW_API renderer_get_should_clear( void )
{
	return shouldClear;
}

dword MEOW_API renderer_create_shader_program( string source )
{
	dword shader = CreateShader( source, 0 );
	dword program = CreateProgram( );
	AttachShader( program, shader );
	LinkProgram( program );
	return program;
}

void MEOW_API renderer_use_program( dword program )
{
	currentProgram = program;
	UseProgram( program );
}

void MEOW_API renderer_delete_program( dword program )
{
	UseProgram( 0 );
	DeleteProgram( program );
}

dword MEOW_API renderer_create_shader( const string source, dword type )
{
	return CreateShader( source, type );
}

dword MEOW_API renderer_create_program( dword* shaders, int32 length )
{
	dword program = CreateProgram( );
	do
	{
		length--;
		AttachShader( program, shaders[ length ] );
	} while ( length );
	LinkProgram( program );
	return program;
}

dword MEOW_API renderer_uniform_location( dword program, const string name )
{
	return UniformLocation( program, name );
}

mem_ptr  MEOW_API renderer_create_buffer_object( float *vertex, float *texture, float *color, dword count )
{
	dword *arrays = malloc( sizeof( dword ) * 3 );
	CreateVBOs( arrays );
	FillVBO( arrays[ 0 ], 3, count, vertex );
	FillVBO( arrays[ 1 ], 2, count, texture );
	FillVBO( arrays[ 2 ], 4, count, color );
	return arrays;
}

void MEOW_API renderer_cleanup_buffer_object( mem_ptr object )
{
	DeleteVBOs( object );
	free( object );
}

void MEOW_API renderer_draw_buffer_object( mem_ptr object, dword count )
{
	RenderVBOs( object, count );
}

void MEOW_API renderer_translate( float x, float y )
{
	glTranslatef( x, y, 0.0f );
}

void MEOW_API renderer_rotate( float angle )
{
	glRotatef( angle, 0.0f, 0.0f, 1.0f );
}

void MEOW_API renderer_push_matrix( void )
{
	glPushMatrix( );
}
void MEOW_API renderer_pop_matrix( void )
{
	glPopMatrix( );
}

void MEOW_API renderer_reset_matrix( void )
{
	glLoadIdentity( );
}