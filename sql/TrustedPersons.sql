create table TrustedPersons(
    ID int NOT NULL AUTO_INCREMENT,
    GuildId int NOT NULL,
    UserId int NOT NULL,
    PRIMARY KEY(ID),
    FOREIGN KEY (GuildId) REFERENCES Guilds(ID),
    FOREIGN KEY (UserId) REFERENCES Users(ID)
);