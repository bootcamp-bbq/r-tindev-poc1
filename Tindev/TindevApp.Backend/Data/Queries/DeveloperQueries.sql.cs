namespace TindevApp.Backend.Data.Queries
{
    public static class DeveloperQueries
    {
        
        public const string All = @"
            SELECT id, name, username, bio, avatar_uri, github_uri, created_at, modified_at 
            FROM Developers
            ORDER BY id;
        ";
        
        public const string CountAll = @"
            SELECT count(1) count 
            FROM Developers;
        ";

        public const string FindById = @"
            SSELECT id, name, username, bio, avatar_uri, github_uri, created_at, modified_at 
            FROM Developers
            WHERE id = @Id;
        ";

        public const string Create = @"
            INSERT INTO Developers (id, name, username, bio, avatar_uri, github_uri, created_at, modified_at)
            values (@Id, @Name, @Username, @Bio, @AvatarUri, @GithubUri, @CreatedAt, @ModifiedAt);
            SELECT @Id;
        ";

        public const string DeleteById = @"
            DELETE Developers
            WHERE id = @Id
        ";
        
        public const string UpdateById = @"
            UPDATE Developers
            set name = @Name, 
                username = @Username, 
                bio = @Bio,
                avatar_uri = @AvatarUri,
                github_uri = @GithubUri,
                modified_at = @ModifiedAt
            WHERE id = @Id\
        ";

    }
}
