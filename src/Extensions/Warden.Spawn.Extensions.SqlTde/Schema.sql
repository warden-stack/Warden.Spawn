CREATE TABLE Configurations
(
	Hash nvarchar(400) primary key not null,
	Warden nvarchar(MAX) not null,
	Name nvarchar(MAX) not null,
	Value nvarchar(MAX) not null,
	Salt nvarchar(MAX) not null,
	Watcher nvarchar(MAX) null,
	Integration nvarchar(MAX) null,
	Hook nvarchar(MAX) null,
	CreatedAt datetime not null
)