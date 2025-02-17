import { PromiseExecutor } from '@nx/devkit';
import { GenerateExecutorSchema } from './schema';
import { generate, Indent } from '../index';

const runExecutor = async (options: GenerateExecutorSchema, context: PromiseExecutor) => {
  console.log(`🚀 Generating OpenAPI spec for: 📜 ${options.input}`);

  try {
    await generate({
      input: options.input,
      output: options.output,
      clientName: options.clientName,
      useOptions: options.useOptions ?? false,
      useUnionTypes: options.useUnionTypes ?? false,
      exportCore: options.exportCore ?? true,
      exportServices: options.exportServices ?? true,
      exportModels: options.exportModels ?? true,
      exportSchemas: options.exportSchemas ?? false,
      indent: options.indent ?? Indent.SPACE_4,
      postfixServices: options.postfixServices ?? 'Service',
      postfixModels: options.postfixModels ?? '',
      request: options.request,
      write: options.write ?? true,
    });

    if (options.verbose) {
      console.log('✅ OpenAPI generation completed successfully!');
    }

    return { success: true };
  } catch (error) {
    const errorMessages = [
      `💥 OpenAPI generation failed! Maybe... start the API first? 🤔`,
      `🚨 OpenAPI generation crashed! Did you forget to run the server? 😅`,
      `🔥 Something went *very* wrong... Is your API even awake? 😴`,
      `❌ OpenAPI generation failed! Try checking if the API exists... 👀`,
      `⚠️ API generation imploded. Is the server just *pretending* to run? 🤷‍♂️`,
      `💀 OpenAPI spec died. CPR won't help, but maybe starting the API will. 🏥`,
      '⚠️  ERROR: OpenAPI generation failed!  \n❌ Is the API... you know... actually running? 🏃💨  \nMaybe try: 👉  *starting the server*'
    ];

    console.error(errorMessages[Math.floor(Math.random() * errorMessages.length)]);

    if (options.verbose) {
      console.error('🛑 Error details:', error);
    }

    return { success: false };
  }
};

export default runExecutor;
