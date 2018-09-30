#include <stdlib.h>
#include "../api.h"

#include "list.h"

list_node* list_create( )
{
	list_node *ret = malloc( sizeof( list_node ) );
	if ( !ret )
		return NULLPTR;
	ret->item = 0;
	ret->next = 0;

	return ret;
}

void list_insert( list_node *node, void *item )
{
	list_node *new = malloc( sizeof( list_node ) );
	if ( !new )
		return;
	new->item = item;
	new->next = node->next;
	node->next = new;
}

void list_remove( list_node *node, void *item )
{
	list_node *tmp = node;
	list_node *prev = node;
	while ( tmp->item != item )
	{
		prev = tmp;
		tmp = tmp->next;
	}
	prev->next = tmp->next;
	free( tmp );
}

void list_cleanup( list_node *node )
{
	if ( node->next != 0 )
		list_cleanup( node->next );
	free( node );
}