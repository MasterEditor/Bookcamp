/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
    "node_modules/flowbite-react/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      keyframes: {
        line: {
          "0%": { transform: "scaleX(0)" },
          "100%": { transform: "scaleX(1)" },
        },
        wiggle: {
          "0%, 100%": { transform: "rotate(-1deg)" },
          "50%": { transform: "rotate(1deg)" },
        },
      },
      animation: {
        "pulse-slow": "pulse 4s cubic-bezier(0.4, 0, 0.6, 1) infinite",
        line: "line 500ms ease-in-out forwards",
        wiggle: "wiggle 200ms ease-in-out",
      },
      boxShadow: {
        button: "5px 5px #c65c60",
      },
    },
    fontFamily: {
      body: ["montserrat", "sans-serif"],
      sans: ["ui-sans-serif", "system-ui"],
    },
  },
  plugins: [require("flowbite/plugin")],
};
