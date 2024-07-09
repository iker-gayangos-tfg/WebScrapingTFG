while ! nc -z db_host 1433; do   
  sleep 1
done

dotnet ef database update --project WebScrapingAPI --context ApplicationDbContext
