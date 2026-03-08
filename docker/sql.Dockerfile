# docker/sql.Dockerfile
FROM mcr.microsoft.com/mssql/server:2025-latest

USER root

ENV ACCEPT_EULA=Y
ENV MSSQL_TCP_PORT=1433

# Install tools + ICU and a few common native deps
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       unzip wget ca-certificates netcat-openbsd \
       libicu74 libssl3 zlib1g libkrb5-3 libgssapi-krb5-2 \
    && ln -sf /usr/lib/x86_64-linux-gnu/libicui18n.so.74 /usr/lib/x86_64-linux-gnu/libicui18n.so \
    && ln -sf /usr/lib/x86_64-linux-gnu/libicuuc.so.74   /usr/lib/x86_64-linux-gnu/libicuuc.so \
    && ln -sf /usr/lib/x86_64-linux-gnu/libicudata.so.74 /usr/lib/x86_64-linux-gnu/libicudata.so \
    && rm -rf /var/lib/apt/lists/*

# Always fetch the latest Linux x64 SqlPackage bundle
RUN wget -O /tmp/sqlpackage.zip https://aka.ms/sqlpackage-linux \
    && unzip /tmp/sqlpackage.zip -d /opt/sqlpackage \
    && chmod +x /opt/sqlpackage/sqlpackage \
    && rm -f /tmp/sqlpackage.zip

# BACPAC + scripts
COPY ./docker/data/demoTraining.bacpac /tmp/db/demoTraining.bacpac
COPY ./docker/build-script/SetupDatabases.sh /usr/local/bin/SetupDatabases.sh
COPY ./docker/build-script/entrypoint.sh     /usr/local/bin/entrypoint.sh
RUN chmod +x /usr/local/bin/*.sh

EXPOSE 1433
ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]