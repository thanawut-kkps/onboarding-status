$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[$("[name='" + this.name + "']").attr("name")] !== undefined) {
            if (!o[$("[name='" + this.name + "']").attr("name")].push) {
                o[$("[name='" + this.name + "']").attr("name")] = [o[$("[name='" + this.name + "']").attr("name")]];
            }
            o[$("[name='" + this.name + "']").attr("name")].push(this.value || '');
        } else {
            o[$("[name='" + this.name + "']").attr("name")] = this.value || '';
        }
    });
    return o;
};

$.postify = function (value) {
    //http://www.nickriggs.com/posts/post-complex-javascript-objects-to-asp-net-mvc-controllers/
    //$.postify(myObject)
    //$.postify({ person: myPerson, otherParam: true })

    var result = {};

    var buildResult = function (object, prefix) {
        for (var key in object) {

            var postKey = isFinite(key)
                ? (prefix != "" ? prefix : "") + "[" + key + "]"
                : (prefix != "" ? prefix + "." : "") + key;

            switch (typeof (object[key])) {
                case "number": case "string": case "boolean":
                    result[postKey] = object[key];
                    break;

                case "object":
                    if (object[key].toUTCString)
                        result[postKey] = object[key].toUTCString().replace("UTC", "GMT");
                    else {
                        buildResult(object[key], postKey != "" ? postKey : key);
                    }
            }
        }
    };

    buildResult(value, "");

    return result;
};
