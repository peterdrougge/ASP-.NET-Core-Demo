	FROM microsoft/dotnet  
 	# set environment variables 
	ENV ASPNETCORE_URLS="http://*:5000" 
	ENV ASPNETCORE_ENVIRONMENT="Development"  
	# copy files to app directory 
	COPY . /app  
	# set working directory 
	WORKDIR /app 
	# restore packages 
	RUN ["dotnet", "restore"] 
	# open port 
	EXPOSE 5000 
	# run the app 
	ENTRYPOINT ["dotnet", "run"] 
