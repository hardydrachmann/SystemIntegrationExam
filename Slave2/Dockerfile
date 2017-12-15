FROM microsoft/aspnet:1.0.0-rc1-update1

COPY . /slave2
WORKDIR /slave2

# Hent basiske .net dependencies fra NuGet (læser fra package.json)
RUN ["dnu", "restore"]

ENTRYPOINT ["dnx","Slave2"]