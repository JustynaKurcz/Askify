FROM node:20-alpine
WORKDIR /app
COPY AskifyClient/package*.json ./
RUN npm install
COPY AskifyClient/ .
EXPOSE 4200
CMD ["npm", "start", "--", "--host", "0.0.0.0"]