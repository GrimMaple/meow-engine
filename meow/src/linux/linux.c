/* - linux.c ---------------------------------------------------------------------------------------
* Linux-specific window implementation
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

#include <X11/Xlib.h>
#include <GL/glx.h>
#include <X11/extensions/xf86vmode.h>
#include <X11/keysym.h>

#include "../window_shared.h"
#include "../renderer.h"
#include "../events.h"

#define KEYMAP(a, b) keys[a] = b
#define VK(a) keys[a]

dword keys[256];

dword lastKey = 0;

Display              *display;
Window                window;
dword                 screen;
GLXContext            context;
XSetWindowAttributes  attr;
XF86VidModeModeInfo   deskMode;
XEvent                event;

static dword attributesList[] = { GLX_RGBA, GLX_DOUBLEBUFFER, 
    GLX_RED_SIZE, 4, 
    GLX_GREEN_SIZE, 4, 
    GLX_BLUE_SIZE, 4, 
    GLX_DEPTH_SIZE, 16,
    None };


void InitKeymaps( void )
{
	KEYMAP(9,  0x1b); // Escape
	KEYMAP(67, 0x70); // F1
	KEYMAP(68, 0x71); // F2
	KEYMAP(69, 0x72); // F3
	KEYMAP(70, 0x73); // F4
	KEYMAP(71, 0x74); // F5
	KEYMAP(72, 0x75); // F6
	KEYMAP(73, 0x76); // F7
	KEYMAP(74, 0x77); // F8
	KEYMAP(75, 0x78); // F9
	KEYMAP(76, 0x79); // F10
	KEYMAP(95, 0x7a); // F11
	KEYMAP(96, 0x7b); // F12
	KEYMAP(10, 0x31); // 1
	KEYMAP(11, 0x32); // 2
	KEYMAP(12, 0x33); // 3
	KEYMAP(13, 0x34); // 4
	KEYMAP(14, 0x35); // 5
	KEYMAP(15, 0x36); // 6
	KEYMAP(16, 0x37); // 7
	KEYMAP(17, 0x38); // 8
	KEYMAP(18, 0x39); // 9
	KEYMAP(19, 0x30); // 0
	KEYMAP(20, 0x6d); // -
	KEYMAP(21, 0x6b); // =
	KEYMAP(22, 0x08); // Backspace
	KEYMAP(23, 0x09); // Tab
	KEYMAP(24, 0x51); // Q
	KEYMAP(25, 0x57); // W
	KEYMAP(26, 0x45); // E
	KEYMAP(27, 0x52); // R
	KEYMAP(28, 0x54); // T
	KEYMAP(29, 0x59); // Y
	KEYMAP(30, 0x55); // U
	KEYMAP(31, 0x49); // I
	KEYMAP(32, 0x4f); // O
	KEYMAP(33, 0x50); // P
	KEYMAP(66, 0x14); // CapsLock
	KEYMAP(38, 0x41); // A
	KEYMAP(39, 0x53); // S
	KEYMAP(40, 0x44); // D
	KEYMAP(41, 0x46); // F
	KEYMAP(42, 0x47); // G
	KEYMAP(43, 0x48); // H
	KEYMAP(44, 0x4a); // J
	KEYMAP(45, 0x4b); // K
	KEYMAP(46, 0x4c); // L
	KEYMAP(36, 0x0d); // Enter
	KEYMAP(50, 0xa0); // LShift
	KEYMAP(52, 0x5a); // Z
	KEYMAP(53, 0x58); // X
	KEYMAP(54, 0x43); // C
	KEYMAP(55, 0x56); // V
	KEYMAP(56, 0x42); // B
	KEYMAP(57, 0x4e); // N
	KEYMAP(58, 0x4d); // M
	KEYMAP(62, 0xa1); // RShift
	KEYMAP(37, 0xa2); // LCtrl
	KEYMAP(133, 0x5b); // LWin
	KEYMAP(65, 0x20); // SPACE
	KEYMAP(135, 0xa5); // RMenu
	KEYMAP(105, 0xa3); // RCtrl
	KEYMAP(113, 0x25); // Left
	KEYMAP(111, 0x26); // Up
	KEYMAP(116, 0x28); // Dowdn
	KEYMAP(114, 0x27); // Right
}

dword WindowInit( void )
{
	InitKeymaps( );
	XVisualInfo *visualInfo;
	int32        width, height;
	Atom         wmDelete;
	Colormap     colorMap;
	window_get_size( &width, &height );
	display = XOpenDisplay( 0 );
	screen = XDefaultScreen( display );
	visualInfo = glXChooseVisual( display, window, attributesList );
	colorMap = XCreateColormap( display, RootWindow( display, visualInfo->screen ), visualInfo->visual, AllocNone );
	attr.colormap = colorMap;
	attr.border_pixel = 0;
	if(!visualInfo)
		return -3;
	context = glXCreateContext(display, visualInfo, 0, GL_TRUE);
    attr.event_mask = PointerMotionMask | ExposureMask | KeyPressMask | KeyReleaseMask | ButtonPressMask | ButtonReleaseMask | StructureNotifyMask;
    window = XCreateWindow( display, RootWindow( display, visualInfo->screen ),
            0, 0, width, height, 0, visualInfo->depth, InputOutput, visualInfo->visual,
            CWBorderPixel | CWColormap | CWEventMask, &attr );
    wmDelete = XInternAtom(display, "WM_DELETE_WINDOW", True);
    XSetWMProtocols(display, window, &wmDelete, 1);
    XSetStandardProperties( display, window, "",
            "", None, NULL, 0, NULL);
    XMapRaised(display, window);

    glXMakeCurrent(display, window, context);
    return 0;  

}

dword WindowCleanup( void )
{
	if (context)
    {
        if ( !glXMakeCurrent( display, None, NULL ) )
        {
            return -1;
        }
        glXDestroyContext( display, context );
        context = NULL;
    }
    XCloseDisplay( display );
}

void SendSizeMessage( dword w, dword h )
{
	// TODO: implement
}

void SendKeyRelease( void )
{
	RaiseEvent( EVT_KEY_UP, VK(lastKey), 0 );
	lastKey = 0;
}

dword GetMouseButtonEvent( int32 button, bool up )
{
	switch( button )
	{
		case Button1:
			return up ? EVT_LMB_UP : EVT_LMB_DOWN;
		case Button3:
			return up ? EVT_RMB_UP : EVT_RMB_DOWN;
	}
}

void ProcessMessages( void )
{
	SendKeyRelease( );
	while( XPending( display ) > 0 )
	{
		XNextEvent( display, &event );
		switch( event.type )
		{
		case ConfigureNotify:
			ChangeSize( event.xconfigure.width, event.xconfigure.height );
			RendererSetup( event.xconfigure.width, event.xconfigure.height );
			break;
		case KeyPress:
			printf("KeyPress: %i\n", event.xkey.keycode);
			if( lastKey == event.xkey.keycode )
				lastKey = 0;
			RaiseEvent( EVT_KEY_DOWN, VK(event.xkey.keycode), 0 );
			break;
		case KeyRelease:
			if( lastKey )
				SendKeyRelease( );
			lastKey = event.xkey.keycode;

			printf("KeyRelease: %i\n", event.xkey.keycode);
			break;
		case MotionNotify:
			RaiseEvent( EVT_MOUSE_MOVE, event.xmotion.x, event.xmotion.y );
			break;
		case ButtonPress:
			RaiseEvent( GetMouseButtonEvent( event.xbutton.button, FALSE ), event.xbutton.x, event.xbutton.y );
			break;
		case ButtonRelease:
			RaiseEvent( GetMouseButtonEvent( event.xbutton.button, TRUE ), event.xbutton.x, event.xbutton.y );
			break;
		case ClientMessage:
        	if (*XGetAtomName(display, event.xclient.message_type) == 
                *"WM_PROTOCOLS")
            {
            	window_break_cycle( );
            }
            break;
		}
	}
}

void SetFullscreen( bool fs )
{
	// TODO: Implement
}

void End( void )
{
	// TODO: implement
	// glFlush( );
	glXSwapBuffers( display, window );
}

void MEOW_API window_set_text( char* text )
{
	XStoreName( display, window, text );
}