import { defineConfig } from "@hey-api/openapi-ts";

export default defineConfig({
    input: "./openapi/v1.json",
    output: {
        path: "src/clients/ProductService",
        format: "prettier",
        lint: "eslint",
    },
    plugins: ["@hey-api/client-axios"],
});
