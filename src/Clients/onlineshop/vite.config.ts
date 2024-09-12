import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
// export default defineConfig(({
//   plugins: [react()],
// }) => {
//   return {

//   }
// })
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");
  return {
    plugins: [react()],
    server: {
      port: Number(env.PORT) || undefined,
    },
    // vite config
    define: {
      __APP_ENV__: JSON.stringify(env.APP_ENV),
    },
  };
});
