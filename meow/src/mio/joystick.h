#ifndef _JOYSTICK_H_
#define _JOYSTICK_H_

#include "../api.h"

#define JOY_BTN_Y      0
#define JOY_BTN_X      1
#define JOY_BTN_A      2
#define JOY_BTN_B      3
#define JOY_DPAD_DOWN  4
#define JOY_DPAD_UP    5
#define JOY_DPAD_LEFT  6
#define JOY_DPAD_RIGHT 7
#define JOR_BTN_LS     8
#define JOY_BTN_RS     9
#define JOY_BTN_LT     10
#define JOY_BTN_RT     11
#define JOY_BTN_START  12
#define JOY_BTN_BACK   13

#define AXIS_LT       0x01000000
#define AXIS_RT       0x02000000
#define AXIS_LX       0x03000000
#define AXIS_LY       0x04000000
#define AXIS_RX       0x05000000
#define AXIS_RY       0x06000000

void MEOW_API joystick_init( void );
void MEOW_API joystick_cycle( dword );

#endif