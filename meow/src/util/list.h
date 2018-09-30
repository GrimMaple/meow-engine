#ifndef _LIST_H_
#define _LIST_H_

typedef struct node
{
	struct node *next;
	void        *item;
} list_node;


list_node* list_create( );
void list_insert( list_node *node, void *item );
void list_remove( list_node *node, void *item );
void list_cleanup( list_node *node );

#endif