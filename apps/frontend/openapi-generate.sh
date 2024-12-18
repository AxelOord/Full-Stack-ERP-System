echo "🔑 Disabling NODE_TLS_REJECT_UNAUTHORIZED to allow introspecting HTTPS API."
export NODE_TLS_REJECT_UNAUTHORIZED=0

npx openapi -i http://127.0.0.1:5076/openapi/v1.json -o src/services