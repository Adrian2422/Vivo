
FROM node:22-alpine AS build
WORKDIR /app
COPY src/Vivo.Web/package*.json ./
RUN npm ci
COPY src/Vivo.Web/ .
RUN npm run build -- --configuration production

FROM nginx:alpine AS final

RUN printf 'server {\n\
    listen 80;\n\
    location / {\n\
        root /usr/share/nginx/html;\n\
        index index.html index.htm;\n\
        try_files $uri $uri/ /index.html;\n\
    }\n\
}' > /etc/nginx/conf.d/default.conf

COPY --from=build /app/dist/Vivo.Web/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
