import { defineConfig } from "@hey-api/openapi-ts";

export default defineConfig({
  client: "@hey-api/client-axios",
  input: "src/clients/authService/specs/swagger.json",
  output: {
    path: "src/clients/authService",
    format: "prettier",
    lint: "eslint",
  },
});
