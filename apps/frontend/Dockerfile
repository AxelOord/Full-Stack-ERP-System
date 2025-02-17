# Step 1: Use official Node.js image
FROM node:18-alpine AS builder

# Set working directory inside container
WORKDIR /app

# Copy only necessary files for better caching
COPY dist/apps/frontend ./

# Install dependencies at the root (Nx handles dependencies)
RUN npm install

# Step 2: Create a minimal image for production
FROM node:18-alpine AS runner
# Set working directory
WORKDIR /app
COPY --from=builder /app/.next/standalone ./standalone

# Expose port 3000
EXPOSE 3000

# Run Next.js in production mode
CMD ["node", "standalone/apps/frontend/server.js"]