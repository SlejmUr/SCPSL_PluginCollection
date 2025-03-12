using CommandSystem;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using UnityEngine;

namespace DavaCustomItems.Commands.LightCommands;

[CommandHandler(typeof(BaseLightCommand))]
public class CreateLightCommand : ICommand, IUsageProvider
{
    public string Command => "create";

    public string[] Aliases => ["c"];

    public string Description => "Creating a Light Source";

    public string[] Usage => ["Position (Id/X,Y,Z)", "Configuration (Optional [use help to list it])"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count < 1 || arguments.Count > 2)
        {
            response = "Are you missing an argument? Use the position with UserId (2) or use as coordinate (34,414,450) Do not put space in there!";
            return false;
        }
        Vector3 position = Vector3.one;
        string pos = arguments.At(0);
        if (pos.Contains(','))
        {
            if (!CommandParseHelper.ParseVector3(pos, ',', out position, out response))
                return false;
        }
        else
        {
            position = Player.Get(int.Parse(pos)).Position;
        }
        Configs.LightConfig lightConfig = new()
        { 
            ShouldMakeLight = true,
            Color = Color.white,
            Scale = Vector3.one
        };
        if (arguments.Count == 2)
        {
            string args = arguments.At(1);
            if (args.Contains("help"))
            {
                response = $"Dont forget If you do not specify anything it will be default valur!\nArgument for Configration would look like: \"follow:true,intensity:10,color:0.4-0.7-0.9-0.5, OR color:0.4-0.7-0.9,scale:1-1-1,lightshape:box,shadowtype:soft,\"";
                return false;
            }
            string[] splitted = [];
            if (!args.Contains(','))
            {
                splitted = [args];
            }
            foreach (string arg in splitted)
            {
                // split between : because one is key, other is value
                var arg_split = arg.Split(':');
                switch (CommandParseHelper.GetInputTypeForLight(arg_split[0]))
                {
                    case CommandParseHelper.InputType.Float:
                        {
                            if (!float.TryParse(arg_split[1], out float result))
                            {
                                response = $"{arg_split[0]} cannot parse as float!";
                                return false;
                            }
                            CommandParseHelper.SetSettingForLight(ref lightConfig, arg_split[0], result);
                        }
                        break;
                    case CommandParseHelper.InputType.Vector3:
                        {
                            Vector3 vector3 = Vector3.one;
                            if (!CommandParseHelper.ParseVector3(arg_split[1], '-', out vector3, out response))
                            {
                                return false;
                            }
                            CommandParseHelper.SetSettingForLight(ref lightConfig, arg_split[0], vector3);
                        }
                        break;
                    case CommandParseHelper.InputType.Color:
                        {
                            Color color = Color.clear;
                            if (!CommandParseHelper.ParseColor(arg_split[1], '-', out color, out response))
                            {
                                return false;
                            }
                            CommandParseHelper.SetSettingForLight(ref lightConfig, arg_split[0], color);
                        }
                        break;
                    case CommandParseHelper.InputType.Bool:
                        {
                            if (!bool.TryParse(arg_split[1], out bool result))
                            {
                                response = "Bool cannot be converted";
                                return false;
                            }
                            CommandParseHelper.SetSettingForLight(ref lightConfig, arg_split[0], result);
                        }
                        break;
                    case CommandParseHelper.InputType.Number:
                        {
                            if (!byte.TryParse(arg_split[1], out byte result))
                            {
                                response = "Byte cannot be converted";
                                return false;
                            }
                            CommandParseHelper.SetSettingForLight(ref lightConfig, arg_split[0], result);
                        }
                        break;
                    case CommandParseHelper.InputType.Enum:
                        {
                            int enum_parsed = CommandParseHelper.TryParseToEnum(arg_split[0], arg_split[1]);
                            CommandParseHelper.SetSettingForLight(ref lightConfig, arg_split[0], enum_parsed);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        int id = LightManager.MakeLight(position, lightConfig);
        response = $"Light has been made with ID: {id}";
        return true;
    }


}
/*
 * args would be like
 * follow:true,
 * intensity:10,
 * range:15,
 * spotrange:10
 * innerspotrange:1,
 * shadowstrength:3,
 * color:0.4-0.7-0.9-0.5, OR color:0.4-0.7-0.9,
 * lightshape:box,
 * lighttype:area
 * shadowtype:soft,
 * movementsmoothing:60,
 * scale:1-1-1,
 * 
 */
