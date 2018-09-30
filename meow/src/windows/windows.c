/* - windows.c -------------------------------------------------------------------------------------
* Windows-specific window implementation
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2016-2017
* --------------------------------------------------------------------------------------------------
*/

#include "..\window_shared.h"
#include "..\events.h"
#include "windows.h"
#include "..\renderer.h"


#define CLASSNAME "MeowGL"

HINSTANCE hInstance;
HWND hWnd;
HDC hDC;
HGLRC hRC;
MSG msg;

LRESULT WINAPI WndProc( HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam )
{
	int32 w, h;
	dword sc, r;
	char state[ 256 ];
	wchar_t string[ 256 ];
	switch ( uMsg )
	{
	case WM_KEYDOWN:
		GetKeyboardState( state );
		sc = MapVirtualKey( wParam, 0 );
		r = ToUnicode( wParam, sc, state, string, 256, 0 );
		RaiseEvent( EVT_KEY_DOWN, wParam, string[ 0 ] );
		return 0;
	case WM_KEYUP:
		RaiseEvent( EVT_KEY_UP, wParam, 0 );
		return 0;
	case WM_MOUSEMOVE:
		RaiseEvent( EVT_MOUSE_MOVE, GET_X_LPARAM( lParam ), GET_Y_LPARAM( lParam ) );
		return 0;
	case WM_LBUTTONDOWN:
		RaiseEvent( EVT_LMB_DOWN, GET_X_LPARAM( lParam ), GET_Y_LPARAM( lParam ) );
		return 0;
	case WM_LBUTTONUP:
		RaiseEvent( EVT_LMB_UP, GET_X_LPARAM( lParam ), GET_Y_LPARAM( lParam ) );
		return 0;
	case WM_RBUTTONDOWN:
		RaiseEvent( EVT_RMB_DOWN, GET_X_LPARAM( lParam ), GET_Y_LPARAM( lParam ) );
		return 0;
	case WM_RBUTTONUP:
		RaiseEvent( EVT_RMB_UP, GET_X_LPARAM( lParam ), GET_Y_LPARAM( lParam ) );
		return 0;
	case WM_CLOSE:
		PostQuitMessage( 0 );
		return 0;
	case WM_SIZE:
		w = LOWORD( lParam ), h = HIWORD( lParam );
		ChangeSize( w, h );
		RendererSetup( w, h );
	default:
		return DefWindowProc( hWnd, uMsg, wParam, lParam );
	}
}

int32 RegisterMeowClass( void )
{
	WNDCLASSEX            wc;

	hInstance = GetModuleHandle( NULL );

	wc.cbSize = sizeof( wc );
	wc.style = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
	wc.lpfnWndProc = WndProc;
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hInstance = hInstance;
	wc.hIcon = LoadIcon( NULL, IDI_APPLICATION );
	wc.hIconSm = wc.hIcon;
	wc.hCursor = LoadCursor( NULL, IDC_ARROW );
	wc.hbrBackground = NULL;
	wc.lpszMenuName = NULL;
	wc.lpszClassName = CLASSNAME;
	if ( !RegisterClassEx( &wc ) )
		return 1;

	return 0;
}

void SendSizeMessage( dword width, dword height )
{
	RECT rc, windowRc;
	GetWindowRect( hWnd, &windowRc );
	GetClientRect( hWnd, &rc );
	rc.right = rc.left + width;
	rc.bottom = rc.top + height;
	AdjustWindowRect( &rc, GetWindowStyle( hWnd ), 0 );
	SetWindowPos( hWnd, NULL, windowRc.left, windowRc.top, rc.right - rc.left, rc.bottom - rc.top, 0 );
	PostMessage( hWnd, WM_SIZE, SIZE_RESTORED, MAKELPARAM( width, height ) );
}

int32 CreateMeowWindow( void )
{
	RECT                  rc;
	DWORD                 exStyle;
	DWORD                 style;

	int32 width, height;
	window_get_size( &width, &height );

	style = WS_OVERLAPPEDWINDOW;
	exStyle = WS_EX_APPWINDOW | WS_EX_WINDOWEDGE;

	rc.left = 0;
	rc.right = width;
	rc.top = 0;
	rc.bottom = height;
	AdjustWindowRectEx( &rc, style, 0, exStyle );

	hWnd = CreateWindow( CLASSNAME, CLASSNAME,
		style,
		100, 100,
		rc.right - rc.left,
		rc.bottom - rc.top,
		NULL, NULL, hInstance, NULL );
	if ( !hWnd )
		return 1;
	
	return 0;
}

PIXELFORMATDESCRIPTOR GetPFD( void )
{
	PIXELFORMATDESCRIPTOR pfd;

	pfd.nSize = sizeof( pfd );
	pfd.nVersion = 1;
	pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
	pfd.iPixelType = PFD_TYPE_RGBA;
	pfd.cColorBits = 24;
	pfd.cRedBits = 0;
	pfd.cRedShift = 0;
	pfd.cGreenBits = 0;
	pfd.cGreenShift = 0;
	pfd.cBlueBits = 0;
	pfd.cBlueShift = 0;
	pfd.cAlphaBits = 0;
	pfd.cAlphaShift = 0;
	pfd.cAccumBits = 0;
	pfd.cAccumRedBits = 0;
	pfd.cAccumGreenBits = 0;
	pfd.cAccumBlueBits = 0;
	pfd.cAccumAlphaBits = 0;
	pfd.cDepthBits = 32;
	pfd.cStencilBits = 0;
	pfd.cAuxBuffers = 0;
	pfd.iLayerType = PFD_MAIN_PLANE;
	pfd.bReserved = 0;
	pfd.dwLayerMask = 0;
	pfd.dwVisibleMask = 0;
	pfd.dwDamageMask = 0;

	return pfd;
}

int SetupRenderingContext( void )
{
	PIXELFORMATDESCRIPTOR pfd;
	dword                 pixelFormat;

	pfd = GetPFD( );

	if ( !( hDC = GetDC( hWnd ) ) )
		return 1;

	if ( !( pixelFormat = ChoosePixelFormat( hDC, &pfd ) ) )
		return 2;

	if ( !( SetPixelFormat( hDC, pixelFormat, &pfd ) ) )
		return 3;

	if ( !( hRC = wglCreateContext( hDC ) ) )
		return 4;

	if ( !wglMakeCurrent( hDC, hRC ) )
		return 5;

	return 0;
}




void ProcessMessages( void )
{
	while ( PeekMessage( &msg, NULL, 0, 0, PM_REMOVE ) )
	{
		if ( msg.message == WM_QUIT )
			window_break_cycle( );
		TranslateMessage( &msg );
		DispatchMessage( &msg );
	}
}

void End( void )
{
	SwapBuffers( hDC );
}

void SetFullscreen( bool fs )
{
	if ( fs )
	{
		SetWindowLong( hWnd, GWL_STYLE, 0 );
		SetWindowLong( hWnd, GWL_EXSTYLE, 0 );

		PostMessage( hWnd, WM_SYSCOMMAND, SC_MAXIMIZE, 0 );
		return;
	}

	SetWindowLong( hWnd, GWL_STYLE, WS_OVERLAPPEDWINDOW );
	SetWindowLong( hWnd, GWL_EXSTYLE, WS_EX_APPWINDOW | WS_EX_WINDOWEDGE );

	PostMessage( hWnd, WM_SYSCOMMAND, SC_MAXIMIZE, 0 );
}

void MEOW_API window_set_text( string text )
{
	SetWindowText( hWnd, text );
}

dword WindowCleanup( void )
{
	if ( hRC )
	{
		if ( !wglMakeCurrent( NULL, NULL ) )
			return 1;
		if ( !wglDeleteContext( hRC ) )
			return 2;
	}
	if ( hDC && !ReleaseDC( hWnd, hDC ) )
		return 3;
	if ( hWnd && !DestroyWindow( hWnd ) )
		return 4;
	if ( hInstance && !UnregisterClass( CLASSNAME, hInstance ) )
		return 5;

	return 0;
}

dword WindowInit( void )
{
	if ( RegisterMeowClass( ) )
		return 1;
	if ( CreateMeowWindow( ) )
		return 2;
	if ( SetupRenderingContext( ) )
		return 3;

	ShowWindow( hWnd, SW_SHOWNORMAL );
	SetForegroundWindow( hWnd );
	SetFocus( hWnd );

	dword w, h;
	window_get_size( &w, &h );
	( w, h );
	RendererSetup( w, h );

	return 0;
}