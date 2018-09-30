#ifndef _EXTENSIONS_H_
#define _EXTENSIONS_H_

#ifdef WIN32_LEAN_AND_MEAN
#undef WIN32_LEAN_AND_MEAN
#endif 

#ifdef _WINDOWS
#include <Windows.h>
#endif

#include <GL/gl.h>
#include <glext.h>

#include "api.h"


void ExtensionsInit( void );
dword CreateShader( string source, dword type );
dword CreateProgram( void );
dword AttachShader( dword, dword );
void  LinkProgram( dword );
void  UseProgram( dword );
void  DeleteProgram( dword );
dword UniformLocation( dword, const string );
void  ActiveTexture( dword );
void  CreateVBOs( dword* );
void  FillVBO( dword, dword, dword, mem_ptr );
void  DeleteVBOs( dword* );

void  ResetVBOState( void );

void  RenderVBOs( dword*, dword );

void MEOW_API ex_uniform_1f( dword, float );
void MEOW_API ex_uniform_2f( dword, float, float );
void MEOW_API ex_uniform_3f( dword, float, float, float );
void MEOW_API ex_uniform_4f( dword, float, float, float, float );
void MEOW_API ex_uniform_1i( dword, int32 );
void MEOW_API ex_attach_texture( dword, int32, dword );

#endif