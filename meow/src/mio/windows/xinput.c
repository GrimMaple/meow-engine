#include <Windows.h>
#include <Xinput.h>

#include "xinput.h"
#include "../../events.h"

#define CHECK_TIMEOUT 1000 // Check controllers every n ms


XINPUT_STATE states[ 4 ];
bool         connected[ 4 ];
dword        timeout = 0;

/*
 *  Buttons check order
 *  Deatils on the exact order are in joystick.h
 */
dword checkOrder[ ] =
{
	XINPUT_GAMEPAD_Y,
	XINPUT_GAMEPAD_X,
	XINPUT_GAMEPAD_A,
	XINPUT_GAMEPAD_B,
	XINPUT_GAMEPAD_DPAD_DOWN,
	XINPUT_GAMEPAD_DPAD_UP,
	XINPUT_GAMEPAD_DPAD_LEFT,
	XINPUT_GAMEPAD_DPAD_RIGHT,
	XINPUT_GAMEPAD_LEFT_SHOULDER,
	XINPUT_GAMEPAD_RIGHT_SHOULDER,
	XINPUT_GAMEPAD_LEFT_THUMB,
	XINPUT_GAMEPAD_RIGHT_THUMB,
	XINPUT_GAMEPAD_START,
	XINPUT_GAMEPAD_BACK
};

void CheckControllers( void )
{
	XINPUT_STATE state;
	for ( int i = 0; i < 4; i++ )
	{
		if ( connected[ i ] )
			continue;
		if ( XInputGetState( i, &state ) == ERROR_SUCCESS )
		{
			connected[ i ] = TRUE;
			states[ i ] = state;
		}
	}
}

void RaiseEvents( void )
{
	XINPUT_STATE state;
	for ( int i = 0; i < 4; i++ )
	{
		if ( !connected[ i ] )
			continue;

		if ( XInputGetState( i, &state ) != ERROR_SUCCESS )
		{
			connected[ i ] = FALSE;
			continue;
		}
		for ( int j = 0; j < sizeof( checkOrder ) / sizeof( dword ); j++ )
		{
			bool status = state.Gamepad.wButtons & checkOrder[ j ];
			bool was = states[ i ].Gamepad.wButtons & checkOrder[ j ];
			if ( was == status )
				continue;
			if ( !was )
				RaiseEvent( EVT_JOY_BTN_DOWN, i, j );
			else
				RaiseEvent( EVT_JOY_BTN_UP, i, j );
		}

		if ( state.Gamepad.bLeftTrigger != states[ i ].Gamepad.bLeftTrigger )
			RaiseEvent( EVT_JOY_AXIS, i, AXIS_LT | (state.Gamepad.bLeftTrigger & ~0xFFFF0000) );
		if ( state.Gamepad.bRightTrigger != states[ i ].Gamepad.bRightTrigger )
			RaiseEvent( EVT_JOY_AXIS, i, AXIS_RT | (state.Gamepad.bRightTrigger& ~0xFFFF0000 ) );
		if ( state.Gamepad.sThumbLX != states[ i ].Gamepad.sThumbLX )
			RaiseEvent( EVT_JOY_AXIS, i, AXIS_LX | (state.Gamepad.sThumbLX& ~0xFFFF0000) );
		if ( state.Gamepad.sThumbRX != states[ i ].Gamepad.sThumbRX )
			RaiseEvent( EVT_JOY_AXIS, i, AXIS_RX | (state.Gamepad.sThumbRX & ~0xFFFF0000 ));
		if ( state.Gamepad.sThumbLY != states[ i ].Gamepad.sThumbLY )
			RaiseEvent( EVT_JOY_AXIS, i, AXIS_LY | (state.Gamepad.sThumbLY& ~0xFFFF0000) );
		if ( state.Gamepad.sThumbRY != states[ i ].Gamepad.sThumbRY )
			RaiseEvent( EVT_JOY_AXIS, i, AXIS_RY | (state.Gamepad.sThumbRY& ~0xFFFF0000 ));

		states[ i ] = state;
	}
}

void MEOW_API joystick_init( void )
{
	CheckControllers( );
}

void MEOW_API joystick_cycle( dword passed )
{
	timeout += passed;
	if ( timeout > CHECK_TIMEOUT )
	{
		timeout -= CHECK_TIMEOUT;
		CheckControllers( );
	}
	RaiseEvents( );
}