#include "../audio.h"

#include <stdio.h>

#define TRACE( a ) printf( "%s\n", #a );
#define ERR( a ) if( a < 0 ) { printf("Error: %s\n", snd_strerror( a )); exit(a); }

#define BUFFER_SIZE 1024*10 // 10 KB
#define PERIOD_SIZE 64 //  1KB

void AudioCallback( snd_async_handler_t *handler )
{
	TRACE( async_handle );
	audio_device *device = snd_async_handler_get_callback_private( handler );
	alsa_out *out = device->device;
	snd_pcm_t *pcm_handle = snd_async_handler_get_pcm( handler );
    snd_pcm_sframes_t avail;
    int err;

    avail = snd_pcm_avail_update( pcm_handle );
    //printf("avail=%ld\n", avail);
    dword have = out->block->length - out->block->position;
    if( avail && out->block )
    {
    	TRACE( async_write );
    	snd_pcm_writei( pcm_handle, out->block->data + out->block->position, PERIOD_SIZE > have ? have : PERIOD_SIZE );
    	if( PERIOD_SIZE > have )
    	{
    		out->block->flags = 1;
    		out->block->position = out->block->length;
    		out->block = 0;
    		for(int i=0; i<2; i++)
    			if( device->blocks[i].flags == 2 )
				{
    				out->block = &device->blocks[i];
				}
    	}
    	else
    	{
    		out->block->position += PERIOD_SIZE;
    	}
    }
    else
    {
        for(int i=0; i<2; i++)
    		if( device->blocks[i].flags == 2 )
			{
    			out->block = &device->blocks[i];
			}
    }
}

void Cycle( audio_device *device )
{
	TRACE( cycle );
	snd_pcm_status_t *status;
	alsa_out *out = device->device;
	snd_pcm_t *pcm_handle = device->device->device;
    snd_pcm_sframes_t avail;
    //snd_pcm_status( pcm_handle, status );
    TRACE( get_avail );
	avail = snd_pcm_avail( pcm_handle );
    printf("avail=%d\n", (int)avail);
    //if(avail < 0)
    //	printf("error (%s)\n", snd_strerror( avail ));
    ERR(avail);
    if(avail < 0)
    	return;
    dword have = out->block->length - out->block->position;
    if( avail > 0 && out->block && avail != -5 )
    {
    	TRACE( cycle_write );
    	snd_pcm_writei( pcm_handle, out->block->data + out->block->position, PERIOD_SIZE > have ? have : PERIOD_SIZE );
    	if( PERIOD_SIZE > have )
    	{
    		out->block->flags = 1;
    		out->block->position = out->block->length;
    		out->block = 0;
    		for(int i=0; i<2; i++)
    			if( device->blocks[i].flags == 2 )
				{
    				out->block = &device->blocks[i];
				}
    	}
    	else
    	{
    		out->block->position += PERIOD_SIZE;
    	}
    }
    else if(avail)
    {
        for(int i=0; i<2; i++)
    		if( device->blocks[i].flags == 2 )
			{
    			out->block = &device->blocks[i];
			}
    }
}

int GetFreeHeader( audio_device* device )
{
	if( device-> blocks[ 0 ].flags )
		return 0;
	if( device->blocks[ 1 ].flags )
		return 1;

	return 1;
}

audio_device_ptr MEOW_API audio_open( int32 samples, dword bpc, dword channels, dword blockAlign, dword avgBps )
{
	snd_pcm_hw_params_t *hw;
	snd_pcm_sw_params_t *sw;
	printf("Entering open\n");
	audio_device *ret = malloc( sizeof( audio_device ) );
	memset( ret->blocks, 0, sizeof( block ) * 2 );
	int32 err = snd_pcm_open( &ret->device->device, "default", SND_PCM_STREAM_PLAYBACK, 0 );
	TRACE( open );
	ERR(err);
	err = snd_pcm_hw_params_malloc( &hw );
	TRACE( hw_alloc );
	ERR(err);
	err = snd_pcm_hw_params_any( ret->device->device, hw );
	TRACE( hw_any );
	ERR(err);
	err = snd_pcm_hw_params_set_access( ret->device->device , hw, SND_PCM_ACCESS_RW_INTERLEAVED );
	TRACE( hw_set_access );
	ERR(err);
	err = snd_pcm_hw_params_set_format ( ret->device->device, hw, SND_PCM_FORMAT_S16_LE );
	TRACE( hw_set_format );
	ERR(err);
	int32 tmp = samples;
	err = snd_pcm_hw_params_set_rate_near ( ret->device->device, hw, &tmp, 0 );
	TRACE( hw_set_rate );
	ERR(err);
	err = snd_pcm_hw_params_set_channels ( ret->device->device, hw, channels );
	TRACE( hw_set_channels );
	ERR(err);
	snd_pcm_uframes_t k = BUFFER_SIZE;
	err = snd_pcm_hw_params_set_buffer_size_near ( ret->device->device, hw, &k );
	TRACE( hw_set_buffer );
	ERR(err);
	k = PERIOD_SIZE;
	err = snd_pcm_hw_params_set_period_size_near ( ret->device->device, hw, &k, NULL);
	TRACE( hw_set_period );
	ERR(err);
	err = snd_pcm_hw_params ( ret->device->device, hw );
	TRACE( params );
	ERR(err);
	snd_pcm_hw_params_free ( hw );
	TRACE( free_hw );
	/*
	snd_pcm_sw_params_malloc ( &sw );
	TRACE( sw_malloc );
	snd_pcm_sw_params_current ( ret->device->device , sw );
	TRACE( sw_current );
	err = snd_pcm_sw_params_set_avail_min(ret->device->device, sw, PERIOD_SIZE);
	TRACE( sw_awail_min );
	ERR(err);
	err = snd_pcm_sw_params( ret->device->device, sw );
	TRACE( sw_params );
	ERR(err);
	snd_pcm_sw_params_free( sw );
	TRACE( sw_free );*/
	err = snd_pcm_prepare( ret->device->device );
	TRACE( prepare );
	ERR(err);
	TRACE( open_done );
	AudioInsert( ret );
	return ret;
	// TODO: implement
}

void MEOW_API audio_close( audio_device *device )
{
	snd_pcm_close( device->device->device );
}

void MEOW_API audio_write( audio_device* device, void* data, dword size)
{
	TRACE( write );
	int fhdr = GetFreeHeader( device );
	if ( fhdr == -1 )
	{
		device->block_done = 0;
		return;
	}
	if ( device->blocks[ fhdr ].data != 0 )
		free( device->blocks[ fhdr ].data );
	device->blocks[ fhdr ].data = malloc( size );
	memcpy( device->blocks[ fhdr ].data, data, size );
	device->blocks[ fhdr ].length = size;
	device->blocks[ fhdr ].flags = 2;
}

void MEOW_API audio_stop( audio_device* device )
{
	// TODO: implement
}