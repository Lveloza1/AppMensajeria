# AppMensajeria
Este es un proyecto para la asignatura de Computación Móvil Avanzada. Es acerca de una app de mensajeria realizada en Xamarin que implementa algunos sensores de Xamarin Essenttials.

# Equipo de trabajo
Nefer Blanchar
Dewis Eguis
Jorge Palacio
Luis Veloza

# <- Documentación de la API ->
ChatsController:

	Obtener lista de chats
	[HttpGet]
	GetChats()
	Retorna: Lista de chats

	Agregar chat
	[HttpPost]
	Entrada:Chat
	Retorna: Chat

	Eliminar todos los chats
	[HttpDelete]
	Sin Entrada
	Sin Retorno

MensajesController:

	Obtener lista Mensajes
	[HttpGet]
	Sin Entrada
	Retorna:Lista Mensajes

	Obtener Mensajes del chat
	[HttpGet("{id}")]
	Entrada: ChatID
	Retorna: Lista de Mensajes

	Mensajes del Usuario
	[HttpGet("Usuario/{id}")]
	Entrada: UsuarioID
	Retorna: Lista de Mensajes

	Agregar Mensaje
	[HttpPost]
	Entrada:Mensaje
	Sin Retorno

	Eliminar todos los Mensajes
	[HttpDelete]
	Sin Entrada
	Sin Retorno

UsuarioChatsController:
	
	Obtener lista Usuarios Chats
	[HttpGet]
	Sin Entrada
	Retorna:Lista UsuarioChats

	Obtener lista de Usuarioschats de usuario
	[HttpGet("{id}")]
	Entrada:UsuarioID
	Retorno: ListaUsuarioChats

	Obtener lista de Usuarioschats del Chats
	[HttpGet("chat/{id}")]
	Entrada:ChatID
	Retorno: ListaUsuarioChats
	
	Obtener lista de Grupos del Usuario
	[HttpGet("ChatGrupo/{id}")]
	Entrada:UsuarioID
	Retorno: ListaUsuarioChats

	Agregar UsuarioChats
	[HttpPost]
	Entrada:UsuarioChat
	Sin Retorno
	
	Eliminar todos los UsuariosChats
	[HttpDelete]
	Sin Entrada
	Sin Retorno
	
UsuariosController:

	Obtener lista de Usuarios
	[HttpGet]
	Sin Entrada
	Retorna:Lista Usuarios

	Agregar Usuario y chats privados
	[HttpPost]
	Usuario
	Retorna:Usuario

	Eliminar todos los Usuarios
	[HttpDelete]
	Sin Entrada
	Sin Retorno
