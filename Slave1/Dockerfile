FROM microsoft/aspnet:1.0.0-rc1-update1

COPY . /slave1
WORKDIR /slave1

# Hent basiske .net dependencies fra NuGet (læser fra package.json)
RUN ["dnu", "restore"]

ENTRYPOINT ["dnx","Slave1"]