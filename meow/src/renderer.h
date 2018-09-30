/* - renderer.h ------------------------------------------------------------------------------------
* OpenGL manipulation and state changing
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2016
* --------------------------------------------------------------------------------------------------
*/

#ifndef _RENDERER_H_
#define _RENDERER_H_

#include "api.h"
#include "util/image.h"

void Begin( );
void RendererSetup( dword, dword );

dword    MEOW_API renderer_load_texture( Image* );
void     MEOW_API renderer_draw_array( float*, float*, float*, dword, dword );
void     MEOW_API renderer_draw_array_offs( float*, float*, float*, dword, dword, dword );
void     MEOW_API renderer_set_texture( dword );
void     MEOW_API renderer_set_clear_color( float, float, float, float );
dword    MEOW_API renderer_get_max_texture_size( void );
void     MEOW_API renderer_set_should_clear( bool );
bool     MEOW_API renderer_get_should_clear( void );
dword    MEOW_API renderer_create_shader_program( string );
void     MEOW_API renderer_use_program( dword );
void     MEOW_API renderer_delete_program( dword );
dword    MEOW_API renderer_create_shader( const string, dword );
dword    MEOW_API renderer_create_program( dword*, int32 );
dword    MEOW_API renderer_uniform_location( dword, const string );
mem_ptr  MEOW_API renderer_create_buffer_object( float*, float*, float*, dword );
void     MEOW_API renderer_cleanup_buffer_object( mem_ptr );
void     MEOW_API renderer_draw_buffer_object( mem_ptr, dword );
void     MEOW_API renderer_free_texture( dword );
void     MEOW_API renderer_translate( float, float );
void     MEOW_API renderer_rotate( float );
void     MEOW_API renderer_push_matrix( void );
void     MEOW_API renderer_pop_matrix( void );
void     MEOW_API renderer_reset_matrix( void );

#endif