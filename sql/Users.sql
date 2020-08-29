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