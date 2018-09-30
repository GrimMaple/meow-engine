#include "extensions.h"

#include <stdlib.h>
#include <stdio.h>


#ifdef _WINDOWS
#define PROCADDR(a) a = wglGetProcAddress( #a )
#else
#include <GL/glx.h>
#define PROCADDR(a) a = glXGetProcAddress( #a )
#endif

#define TRACE_ERROR(s) HasErrors() ? printf("Error at %s: %#X\n", s, error) : 1 ;

bool vboEnabled = FALSE;

dword error = 0;
bool HasErrors( )
{
	// FIXME: what an actual fuck?
#ifdef _WINDOWS
	error = glGetError( );
	return error != GL_NO_ERROR;
#endif
	return FALSE;
}

PFNGLCREATESHADERPROC       glCreateShader;
PFNGLCREATEPROGRAMPROC      glCreateProgram;
PFNGLCOMPILESHADERPROC      glCompileShader;
PFNGLSHADERSOURCEPROC       glShaderSource;
PFNGLATTACHSHADERPROC       glAttachShader;
PFNGLLINKPROGRAMPROC        glLinkProgram;
PFNGLUSEPROGRAMPROC         glUseProgram;
PFNGLGETPROGRAMIVPROC       glGetProgramiv;
PFNGLGETSHADERIVPROC        glGetShaderiv;
PFNGLGETSHADERINFOLOGPROC   glGetShaderInfoLog;
PFNGLDELETEPROGRAMPROC      glDeleteProgram;
PFNGLGETUNIFORMLOCATIONPROC glGetUniformLocation;
PFNGLUNIFORM1FPROC          glUniform1f;
PFNGLUNIFORM2FPROC          glUniform2f;
PFNGLUNIFORM3FPROC          glUniform3f;
PFNGLUNIFORM4FPROC          glUniform4f;
PFNGLUNIFORM1IPROC          glUniform1i;
PFNGLGENBUFFERSPROC         glGenBuffers;
PFNGLBINDBUFFERPROC         glBindBuffer;
PFNGLBUFFERDATAPROC         glBufferData;
PFNGLDELETEBUFFERSPROC      glDeleteBuffers;

#ifdef WIN32
PFNGLACTIVETEXTUREPROC      glActiveTexture;
#endif

void UseProgram( dword program )
{
	glUseProgram( program );
}

void shaderLog( unsigned int shader )
{
	int   infologLen = 0;
	int   charsWritten = 0;
	char *infoLog;

	glGetShaderiv( shader, GL_INFO_LOG_LENGTH, &infologLen );

	if ( infologLen > 1 )
	{
		infoLog = malloc( infologLen );
		if ( infoLog == NULL )
		{
			return;
		}
		glGetShaderInfoLog( shader, infologLen, &charsWritten, infoLog );
		printf( "Info log: %s\n", infoLog );
		free(infoLog);
	}
}

void ExtensionsInit( void )
{
	PROCADDR( glCreateShader );
	PROCADDR( glCreateProgram );
	PROCADDR( glShaderSource );
	PROCADDR( glCompileShader );
	PROCADDR( glAttachShader );
	PROCADDR( glLinkProgram );
	PROCADDR( glUseProgram );
	PROCADDR( glGetProgramiv );
	PROCADDR( glGetShaderiv );
	PROCADDR( glGetShaderInfoLog );
	PROCADDR( glDeleteProgram );
	PROCADDR( glGetUniformLocation );
	PROCADDR( glUniform1f );
	PROCADDR( glUniform2f );
	PROCADDR( glUniform3f );
	PROCADDR( glUniform4f );
	PROCADDR( glUniform1i );
	PROCADDR( glGenBuffers );
	PROCADDR( glBindBuffer );
	PROCADDR( glBufferData );
	PROCADDR( glDeleteBuffers );
	#ifdef WIN32
	PROCADDR( glActiveTexture );
	#endif
}

void ActiveTexture( dword id )
{
	//glEnable( GL_TEXTURE );
	//TRACE_ERROR( "glEnable" );
	glActiveTexture( GL_TEXTURE0 + id );
	TRACE_ERROR( "glActiveTexture" );
}

dword CreateShader( string source, dword type )
{
	dword res = glCreateShader( GL_FRAGMENT_SHADER );
	glShaderSource( res, 1, &source, 0 );
	TRACE_ERROR( "glShaderSource" );
	glCompileShader( res );
	TRACE_ERROR( "glCompileShader" );
	shaderLog( res );
	return res;
}

dword CreateProgram( void )
{
	return glCreateProgram( );
}

dword AttachShader( dword program, dword shader )
{
	glAttachShader( program, shader );
	TRACE_ERROR( "glAttachShader" );
	return 0;
}

void  LinkProgram( dword program )
{
	glLinkProgram( program );
	TRACE_ERROR( "glLinkProgram" );
}

void DeleteProgram( dword program )
{
	glDeleteProgram( program );
}

dword UniformLocation( dword program, const string name )
{
	return glGetUniformLocation( program, name );
}

void CreateVBOs( dword *vbos )
{
	glGenBuffers( 3, vbos );
}

void FillVBO( dword vbo, dword size, dword count, mem_ptr data )
{
	vboEnabled = TRUE;
	glBindBuffer( GL_ARRAY_BUFFER, vbo );
	glBufferData( GL_ARRAY_BUFFER, count*size * sizeof( float ), data, GL_STATIC_DRAW );
}

void ResetVBOState( void )
{
	if ( !vboEnabled )
		return;

	glBindBuffer( GL_ARRAY_BUFFER, 0 );
	vboEnabled = FALSE;
}

void DeleteVBOs( dword *object )
{
	glDeleteBuffers( 3, object );
}

void RenderVBOs( dword *vbos, dword count )
{
	vboEnabled = TRUE;
	glBindBuffer( GL_ARRAY_BUFFER, vbos[ 0 ] );
	glVertexPointer( 3, GL_FLOAT, 0, NULLPTR );
	glBindBuffer( GL_ARRAY_BUFFER, vbos[ 1 ] );
	glTexCoordPointer( 2, GL_FLOAT, 0, NULLPTR );
	glBindBuffer( GL_ARRAY_BUFFER, vbos[ 2 ] );
	glColorPointer( 4, GL_FLOAT, 0, NULLPTR );
	glDrawArrays( GL_TRIANGLES, 0, count );
}

void MEOW_API ex_uniform_1f( dword location, float value )
{
	glUniform1f( location, value );
	TRACE_ERROR( "glUniform1f" );
}

void MEOW_API ex_uniform_2f( dword location, float _0, float _1 )
{
	glUniform2f( location, _0, _1 );
	TRACE_ERROR( "glUniform2f" );
}

void MEOW_API ex_uniform_3f( dword location, float _0, float _1, float _2 )
{
	glUniform3f( location, _0, _1, _2 );
	TRACE_ERROR( "glUniform3f" );
}

void MEOW_API ex_uniform_4f( dword location, float _0, float _1, float _2, float _3 )
{
	glUniform4f( location, _0, _1, _2, _3 );
	TRACE_ERROR( "glUniform4f" );
}

void MEOW_API ex_uniform_1i( dword location, int32 value )
{
	glUniform1i( location, value );
	TRACE_ERROR( "glUniform1i" );
}

void MEOW_API ex_attach_texture( dword location, int32 unit, dword texture )
{
	ActiveTexture( unit );
	glBindTexture( GL_TEXTURE_2D, texture );
	TRACE_ERROR( "glBindTexture" );
	ex_uniform_1i( location, unit );
}