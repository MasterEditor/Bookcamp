FROM node:19-alpine AS builder

WORKDIR /app

COPY package-lock.json .
COPY package.json .

RUN yarn install --production

COPY . .

RUN yarn build

FROM nginx:alpine

WORKDIR /usr/share/nginx/html

RUN rm -rf *

COPY --from=builder /app/build .

ENTRYPOINT ["nginx", "-g", "daemon off;"]