echo "🔑 Disabling NODE_TLS_REJECT_UNAUTHORIZED to allow introspecting HTTPS API."
export NODE_TLS_REJECT_UNAUTHORIZED=0

npx openapi -i https://localhost:7013/openapi/v1.json -o src/services