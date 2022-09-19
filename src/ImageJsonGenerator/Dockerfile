FROM mcr.microsoft.com/dotnet/core/runtime:3.1.0
WORKDIR /app
COPY ./out ./
ENV ASPNETCORE_ENVIRONMENT="Production"
RUN apt-get update
RUN apt-get install -y libc6-dev 
RUN apt-get install -y libgdiplus
ENTRYPOINT ["dotnet", "ImageJsonGenerator.dll"]