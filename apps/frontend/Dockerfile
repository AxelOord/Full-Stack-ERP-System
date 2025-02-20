# Base image for installing dependencies
FROM node:18-alpine AS dependencies
WORKDIR /usr/src/app
COPY package.json ./
RUN npm install --omit=dev && rm -rf node_modules/.cache

# Build stage
FROM node:18-alpine AS builder
WORKDIR /usr/src/app
COPY --from=dependencies /usr/src/app/node_modules ./node_modules
COPY . .

# Production stage
FROM node:18-alpine AS runner
WORKDIR /usr/src/app

ENV NODE_ENV=production
ENV PORT=3000
ENV HOST=0.0.0.0
ENV NEXT_TELEMETRY_DISABLED=1

# Copy only necessary files
# --chown to change owner so that runner can cache files
COPY --from=builder --chown=node:node /usr/src/app/.next ./.next
COPY --from=builder /usr/src/app/public ./public
COPY --from=builder /usr/src/app/package.json ./
COPY --from=dependencies /usr/src/app/node_modules ./node_modules

# Run as non-root user
USER node

EXPOSE 3000
CMD ["npm", "start"]