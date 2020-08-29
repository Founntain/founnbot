create table Guilds(
    ID int NOT NULL AUTO_INCREMENT,
    GuildId bigint NOT NULL,
    Prefix varchar(8) NULL,
    PRIMARY KEY(ID)
);

create table Users(
    ID int NOT NULL AUTO_INCREMENT,
    UserId bigint NOT NULL,
    GuildId int NOT NULL,
    TotalXp bigint,
    Xp bigint,
    Level smallint,
    PRIMARY KEY(ID),
    FOREIGN KEY (GuildId) REFERENCES Guilds(ID)
);

create table TrustedPersons(
    ID int NOT NULL AUTO_INCREMENT,
    GuildId int NOT NULL,
    UserId int NOT NULL,
    PRIMARY KEY(ID),
    FOREIGN KEY (GuildId) REFERENCES Guilds(ID),
    FOREIGN KEY (UserId) REFERENCES Users(ID)
);