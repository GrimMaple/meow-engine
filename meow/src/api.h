/* - api.h -----------------------------------------------------------------------------------------
 * An API definition for the whole meow low-level library.
 *
 * This file is a part of MEOW project.
 *
 * This software is in public domain, distributed on "AS IS" basis, without technical support,
 * and with no warranty, epress or implied, as to its usefulness for any purpose.
 *
 * Copyright (c) Grim Maple @ 2016-2017
 * -------------------------------------------------------------------------------------------------
 */

#ifndef _API_H_
#define _API_H_

#include <stdint.h>

#ifdef _WINDOWS
#ifndef MEOW_STATIC
#define MEOW_API  __declspec(dllexport) __stdcall
#else
#define MEOW_API __stdcall
#endif
#else
#define __stdcall //__attribute__((stdcall))
#define MEOW_API //__attribute__((stdcall))
#endif

typedef int8_t int8;
typedef uint8_t uint8, byte;
typedef int16_t int16;
typedef uint16_t uint16, word;
typedef int32_t int32, bool;
typedef uint32_t uin32, dword;
typedef int64_t int64;
typedef uint64_t uint64, qword;
typedef char* string;
typedef void* mem_ptr;

// Grim: Please do something with this typedef on linux
#ifdef _WINDOWS
typedef wchar_t* wstring;
#endif

#ifdef TRUE
#undef TRUE
#endif

#define TRUE 1

#ifdef FALSE
#undef FALSE
#endif

#define FALSE 0

#ifdef NULLPTR
#undef NULLPTR
#endif

#define NULLPTR (void*)0

#endif
