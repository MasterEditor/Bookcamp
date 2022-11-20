FROM node:19-alpine AS builder

WORKDIR /app

COPY ./package.json package.json

RUN npm install

COPY . .

ARG REACT_APP_SERVER_ENDPOINT
ENV REACT_APP_SERVER_ENDPOINT=$REACT_APP_SERVER_ENDPOINT

RUN npm run build

FROM nginx:alpine

WORKDIR /usr/share/nginx/html

RUN rm -rf *

COPY --from=builder /app/build .

RUN rm /etc/nginx/conf.d/default.conf

COPY nginx/nginx.conf /etc/nginx/conf.d

ENTRYPOINT ["nginx", "-g", "daemon off;"]