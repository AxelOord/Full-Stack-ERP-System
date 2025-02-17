const fs = require("fs");
const path = require("path");

// Resolve project root
const projectRoot = path.resolve(__dirname, "../../../");
const destinationDir = path.join(projectRoot, "dist/libs/generate-api");

// Define files and directories to copy
const filesToCopy = [
    "README.md",
    "generators.json",
    "executors.json",
    "package.json",
].map(file => path.join(projectRoot, "libs/generate-api", file));

const directoriesToCopy = [
    path.join(projectRoot, "/libs/generate-api/src/executors")
];
// Ensure destination directory exists
try {
    console.log("Current working directory:", process.cwd());
    if (!fs.existsSync(destinationDir)) {
        fs.mkdirSync(destinationDir, { recursive: true });
        console.log(`📁 Created directory: ${destinationDir}`);
    }

    // Copy individual files
    filesToCopy.forEach(file => {
        if (fs.existsSync(file)) {
            const fileName = path.basename(file);
            const destPath = path.join(destinationDir, fileName);
            fs.copyFileSync(file, destPath);
            console.log(`✅ Copied: ${file} -> ${destPath}`);
        } else {
            console.warn(`⚠️ Warning: Source file does not exist: ${file}`);
        }
    });

    // Recursively copy directories
    directoriesToCopy.forEach(dir => {
        const relativePath = path.relative(path.join(projectRoot, "libs/generate-api"), dir);
        const destPath = path.join(destinationDir, relativePath); // Preserve subdirectory structure
    
        if (fs.existsSync(dir)) {
            fs.cpSync(dir, destPath, { recursive: true });
            console.log(`📂 Copied directory: ${dir} -> ${destPath}`);
        } else {
            console.warn(`⚠️ Warning: Source directory does not exist: ${dir}`);
        }
    });

    console.log("🎉 All assets copied successfully!");
    process.exit(0);
} catch (error) {
    console.error("❌ Error copying assets:", error);
    process.exit(1);
}